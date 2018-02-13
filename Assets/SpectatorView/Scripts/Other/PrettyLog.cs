namespace Holoframework.SimpleTools
{
    public class PrettyLog
    {
        private static Style _style = new Style();

        public static string GetMessage(string @class, string method, string message, object data)
        {
            return string.Format(
                "<b><size={0}><color={1}>[{2}]</color><color={3}>[{4}]</color><color={5}> {6}</color><color={7}> {8}</color></size></b>",
                _style.FontSize, 
                _style.ClassColor, @class, 
                _style.MethodColor, method, 
                _style.MessageColor, message, 
                _style.DataColor, data ?? string.Empty);
        }

        public static void SetStyle(Style style)
        {
            _style = style;
        }

        public static void SetStyle(int fontSize, string classColor, string methodColor, string messageColor, string dataColor)
        {
            _style = new Style(fontSize, classColor, methodColor, messageColor, dataColor);
        }

        public class Style
        {
            public object FontSize { get; private set; }
            public string ClassColor { get; private set; }
            public string MethodColor { get; private set; }
            public string MessageColor { get; private set; }
            public string DataColor { get; private set; }
            
            public Style(
                int fontSize =         12,
                string classColor =    "#0B7E1EFF", 
                string methodColor =   "#0E676F", 
                string messageColor =  "#003653", 
                string dataColor =     "#B40905")
            {
                FontSize = fontSize;
                ClassColor = classColor;
                MethodColor = methodColor;
                MessageColor = messageColor;
                DataColor = dataColor;
            }
        }
    }
}