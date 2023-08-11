namespace ZdravoCorp.Utils.ViewModel
{
    internal class RelayCommand<T>
    {
        private object executeRowDoubleClick;

        public RelayCommand(object executeRowDoubleClick)
        {
            this.executeRowDoubleClick = executeRowDoubleClick;
        }
    }
}