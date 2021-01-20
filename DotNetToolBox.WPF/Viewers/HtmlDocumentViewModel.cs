namespace DotNetToolBox.WPF.Viewers
{
    public class HtmlDocumentViewModel : Observable
    {
        private string _htmlText;
        public string HtmlText
        {
            get => _htmlText;
            set => SetField(ref _htmlText, value);
        }
    }
}