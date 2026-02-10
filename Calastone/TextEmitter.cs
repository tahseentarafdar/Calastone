namespace Calastone
{
    public class TextEmitter: ITextEmitter
    {
        public void Emit(string text)
        {
            Console.Write(text);
        }
    }
}
