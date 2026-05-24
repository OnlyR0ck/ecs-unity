using System.Collections.Generic;
using DCFApixels.DragonECS;
using UnityEngine;
using VContainer;
using VS.Runtime.Core.Components;
using VS.Runtime.Core.Components.OneFrameComponents.Events;
using VS.Runtime.Core.Enums;
using VS.Runtime.Core.Models;
using VS.Runtime.Core.Views;
using VS.Runtime.Extensions;

namespace VS.Runtime.Core.Systems
{
    public class RippleEffectSystem : IEcsRun
    {
        #if ENABLE_IL2CPP
        using Unity.IL2CPP.CompilerServices;
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        #endif
        private class EventAspect : EcsAspect
        {
            public EcsPool<RefreshFieldEvent> Events = Inc;
        }
        
        private class RippleAspect : EcsAspect
        {
            public EcsPool<RippleEffect> Ripples = Inc;
            public EcsPool<UnityComponent<Transform>> Transforms = Inc;
        }
        
        private readonly EcsDefaultWorld _world;
        private readonly GridModel _gridModel;
        
        // Spring parameters
        private const float DefaultStiffness = 200.0f;
        private const float DefaultDamping = 8f;
        private const float DefaultIntensity = 1f;
        private const float MinVelocityThreshold = 0.01f;
        private const float MinDisplacementThreshold = 0.001f;
        
        [Inject]
        public RippleEffectSystem(EcsDefaultWorld world, GridModel gridModel)
        {
            _world = world;
            _gridModel = gridModel;
        }
        
        public void Run()
        {
            // Handle new ripple effects from popped bubbles
            ProcessPoppedBubbles();
            
            // Update existing ripple effects
            UpdateRippleEffects();
        }
        
        private void ProcessPoppedBubbles()
        {
            foreach (int entity in _world.Where(out EventAspect aspect))
            {
                Vector2Int poppedIndex = aspect.Events.Get(entity).Index;
                
                HashSet<Vector2Int> poppedIndices = new HashSet<Vector2Int>();
                _gridModel.GetConnectedSameColoredCells(poppedIndex, ref poppedIndices);
                
                // Find all surrounding bubbles (neighbors of popped bubbles)
                HashSet<Vector2Int> surroundingBubbles = new HashSet<Vector2Int>();
                
                foreach (Vector2Int poppedIdx in poppedIndices)
                {
                    IReadOnlyCollection<Vector2Int> neighbors = _gridModel.GetNeighbors(poppedIdx);
                    
                    foreach (Vector2Int neighborIdx in neighbors)
                    {
                        CellView neighborCell = _gridModel[neighborIdx];
                        
                        // Only add occupied bubbles that are not being popped
                        if (neighborCell.State == ECellState.Occupied && 
                            !poppedIndices.Contains(neighborIdx) &&
                            neighborCell.Content is BubbleView)
                        {
                            surroundingBubbles.Add(neighborIdx);
                        }
                    }
                }
                
                // Get existing ripple transforms to avoid duplicates
                HashSet<Transform> existingRippleTransforms = new HashSet<Transform>();
                foreach (int rippleEntity in _world.Where(out RippleAspect rippleAspect))
                {
                    Transform existingTransform = rippleAspect.Transforms.Get(rippleEntity).obj;
                    if (existingTransform != null)
                    {
                        existingRippleTransforms.Add(existingTransform);
                    }
                }
                
                // Create ripple effects for surrounding bubbles
                // Only affect bubbles that are closest to at least one popped bubble
                foreach (Vector2Int bubbleIdx in surroundingBubbles)
                {
                    CellView cell = _gridModel[bubbleIdx];
                    BubbleView bubbleView = cell.Content as BubbleView;
                    
                    if (bubbleView == null)
                        continue;
                    
                    // Skip if this bubble already has a ripple effect
                    if (existingRippleTransforms.Contains(bubbleView.transform))
                        continue;
                    
                    // Find the closest popped bubble to determine ripple direction
                    Vector2Int closestPoppedIdx = FindClosestPoppedBubble(bubbleIdx, poppedIndices);
                    Vector2 bubblePosition = cell.transform.position;
                    Vector2 sourcePosition = _gridModel[closestPoppedIdx].transform.position;
                    
                    // Calculate initial velocity based on direction from source
                    Vector2 direction = (bubblePosition - sourcePosition).normalized;
                    Vector2 initialVelocity = direction * DefaultIntensity;
                    
                    // Create entity for ripple effect
                    int rippleEntity = _world.NewEntity();
                    
                    ref RippleEffect ripple = ref _world.GetPool<RippleEffect>().TryAddOrGet(rippleEntity);
                    ripple.RestPosition = bubblePosition;
                    ripple.Velocity = initialVelocity;
                    ripple.SourcePosition = sourcePosition;
                    ripple.Stiffness = DefaultStiffness;
                    ripple.Damping = DefaultDamping;
                    ripple.Intensity = DefaultIntensity;
                    
                    // Add transform component
                    ref UnityComponent<Transform> transform = ref _world.GetPool<UnityComponent<Transform>>().TryAddOrGet(rippleEntity);
                    transform.obj = bubbleView.transform;
                }
            }
        }
        
        private Vector2Int FindClosestPoppedBubble(Vector2Int bubbleIdx, HashSet<Vector2Int> poppedIndices)
        {
            Vector2 bubblePos = _gridModel[bubbleIdx].transform.position;
            Vector2Int closestIdx = Vector2Int.zero;
            float closestDistance = float.MaxValue;
            
            foreach (Vector2Int poppedIdx in poppedIndices)
            {
                Vector2 poppedPos = _gridModel[poppedIdx].transform.position;
                float distance = Vector2.SqrMagnitude(bubblePos - poppedPos);
                
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIdx = poppedIdx;
                }
            }
            
            return closestIdx;
        }
        
        private void UpdateRippleEffects()
        {
            float deltaTime = Time.deltaTime;
            
            foreach (int entity in _world.Where(out RippleAspect aspect))
            {
                ref RippleEffect ripple = ref aspect.Ripples.Get(entity);
                Transform transform = aspect.Transforms.Get(entity).obj;
                
                if (transform == null)
                {
                    // Clean up if transform is destroyed
                    aspect.Ripples.Del(entity);
                    aspect.Transforms.Del(entity);
                    continue;
                }
                
                // Spring physics: F = -kx - cv
                // acceleration = -stiffness * displacement - damping * velocity
                Vector2 currentPosition = transform.position;
                Vector2 displacement = currentPosition - ripple.RestPosition;
                Vector2 acceleration = -ripple.Stiffness * displacement - ripple.Damping * ripple.Velocity;
                
                // Update velocity
                ripple.Velocity += acceleration * deltaTime;
                
                // Update position using velocity
                Vector2 newPosition = currentPosition + ripple.Velocity * deltaTime;
                transform.position = newPosition;
                
                // Check if ripple has settled (velocity and displacement are small enough)
                if (ripple.Velocity.sqrMagnitude < MinVelocityThreshold * MinVelocityThreshold &&
                    displacement.sqrMagnitude < MinDisplacementThreshold * MinDisplacementThreshold)
                {
                    // Snap to rest position and remove ripple effect
                    transform.position = ripple.RestPosition;
                    aspect.Ripples.Del(entity);
                    aspect.Transforms.Del(entity);
                }
            }
        }
    }
}
