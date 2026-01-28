namespace Code_explainer
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnMenuClicked(object sender, EventArgs e)
        {
            StatusLabel.Text = "Menu clicked (placeholder)";
        }

        private void OnPlayClicked(object sender, EventArgs e)
        {
            StatusLabel.Text =
                $"kontroll={CheckControl.IsChecked}, loop={CheckLoop.IsChecked}, method={CheckMethod.IsChecked}";
        }
    }
}
