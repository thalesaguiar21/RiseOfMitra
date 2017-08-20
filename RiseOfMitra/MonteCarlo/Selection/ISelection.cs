namespace RiseOfMitra.MonteCarlo.Selection
{
    public interface ISelection
    {
        Node Execute(SelectionParameters parameters);
    }
}
