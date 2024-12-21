namespace VS.Runtime.Core.Components
{
    public struct ViewComponent<T>
    {
        public T View;

        public ViewComponent(T view)
        {
            View = view;
        }
    }
}