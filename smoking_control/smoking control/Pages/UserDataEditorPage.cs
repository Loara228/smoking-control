using smoking_control.Models;

namespace smoking_control.Pages;

public class UserDataEditorPage : ContentPage
{
	public UserDataEditorPage()
	{
        // Спасибо разраб, что ты такой ахуенный. Как же мне повезло юзать шарп в это время. Вы реально уебки, фреемворк поколений. На дебагер, На дебагер, На дебагер сукаааа.
        // Спасибо разраб, что ты такой ахуенный. Как же мне повезло юзать шарп в это время. Вы реально уебки, фреемворк поколений. На дебагер, На дебагер, На дебагер сукаааа.
        // Спасибо разраб, что ты такой ахуенный. Как же мне повезло юзать шарп в это время. Вы реально уебки, фреемворк поколений. На дебагер, На дебагер, На дебагер сукаааа.
        //CarouselView carouselView = new CarouselView
        //{
        //    VerticalOptions = LayoutOptions.Start
        //};

        //carouselView.ItemsSource = new List<Qa>()
        //{
        //    new Qa("Вопрос 1?", "Hint1", "text"),
        //    new Qa("Сложный и длинный вопрос?", "200", "jadhaksdh"),
        //};

        //carouselView.ItemTemplate = new DataTemplate(() =>
        //{
        //    Label header = new Label
        //    {
        //        FontAttributes = FontAttributes.Bold,
        //        HorizontalTextAlignment = TextAlignment.Center,
        //        FontSize = 18
        //    };
        //    header.SetBinding(Label.TextProperty, "Question");

        //    Label label2 = new Label();
        //    label2.SetBinding(Label.TextProperty, "Placeholder");

        //    Label description = new Label { HorizontalTextAlignment = TextAlignment.Center };
        //    description.SetBinding(Label.TextProperty, "Description");

        //    StackLayout stackLayout = new StackLayout() { header, label2, description };
        //    Border frame = new Border();
        //    frame.Content = stackLayout;
        //    return frame;
        //});


        _label1 = new Label()
        {
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,
            FontSize = 18,
            Padding = 10,
        };

        _label2 = new Label()
        {
            FontSize = 14,
            TextColor = Colors.Gray,
            HorizontalOptions = LayoutOptions.Center,
            Padding = 10,
            HorizontalTextAlignment = TextAlignment.Center
        };

        _textbox1 = new Entry();

        _border = new Border()
        {
            Content = new StackLayout()
            {
                _label1,
                _label2,
                _textbox1
            }
        };

        _btnPrev = new Button() { Text = "Back" };
        _btnNext = new Button() { Text = "Next" };
        _btnPrev.Clicked += async (s, e) => { await Previous(); };
        _btnNext.Clicked += async (s, e) => { await Next(); };

        var grid = new Grid
        {
            Padding = 10,
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = GridLength.Auto }
            },
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto},
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = GridLength.Auto}
            },
        };

        grid.Add(_border, 0, 0);
        grid.SetColumnSpan(_border, 3);
        grid.Add(_btnPrev, 0, 2);
        grid.Add(_btnNext, 2, 2);

        Content = grid;
        (Content as Grid)!.Padding = 30;

        SetQuestion(0);
    }

    private async Task Previous()
    {
        if (_onAnimation)
            return;
        if (_curIndex > 0)
        {
            await AnimLeftIn();
            SetQuestion(--_curIndex);
            await AnimLeftOut();
        }
    }

    private async Task Next()
    {
        if (_onAnimation)
            return;
        if (_questions[_curIndex].Keyboard == Keyboard.Numeric)
        {
            if (!Int16.TryParse(_textbox1.Text, out Int16 result) || result <= 0)
            {
                await DisplayAlert("incorrect value", "Check the input field", "OK");
                return;
            }
            _questions[_curIndex].InputText = result.ToString();
        }

        if (_curIndex < _questions.Length - 1)
        {
            await AnimRightIn();
            SetQuestion(++_curIndex);
            await AnimRightOut();
        }
        else
        {
            await Navigation.PopModalAsync();
        }
    }

    private async Task AnimLeftIn()
    {
        _onAnimation = true;
        await Task.WhenAll(
            _border.FadeTo(0, ANIM_TIME, Easing.CubicIn),
            _border.TranslateTo(ANIM_X, 0, ANIM_TIME, Easing.CubicIn),
            Task.Delay(500)
        );
    }

    private async Task AnimLeftOut()
    {
        _border.TranslationX = -ANIM_X;
        await Task.WhenAll(
            _border.FadeTo(1, ANIM_TIME, Easing.CubicOut),
            _border.TranslateTo(0, 0, ANIM_TIME, Easing.CubicOut)
        );
        _onAnimation = false;
    }

    private async Task AnimRightIn()
    {
        _onAnimation = true;
        await Task.WhenAll(
            _border.FadeTo(0, ANIM_TIME, Easing.CubicInOut),
            _border.TranslateTo(-ANIM_X, 0, ANIM_TIME, Easing.CubicIn),
            Task.Delay(500)
        );
    }

    private async Task AnimRightOut()
    {
        _border.TranslationX = ANIM_X;
        await Task.WhenAll(
            _border.FadeTo(1, ANIM_TIME, Easing.CubicIn),
            _border.TranslateTo(0, 0, ANIM_TIME, Easing.CubicOut)
        );
        _onAnimation = false;
    }

    private void SetQuestion(int index)
    {
        var q = _questions[index];

        _label1.Text = q.Question;
        _label2.Text = q.Description;

        _textbox1.Text = q.InputText;
        _textbox1.Placeholder = q.Placeholder;
        _textbox1.MaxLength = q.InputLength;

        _btnPrev.IsEnabled = index != 0;

        if (index == _questions.Length - 1)
        {
            _btnNext.Text = "Save";
        }
        else
        {
            _btnNext.Text = "Next";
        }
    }

    public UserData CollectData()
    {
        var cpd = Int16.Parse(_questions[0].InputText);
        var cc = Int16.Parse(_questions[1].InputText);

        if (cc < 1)
            cc = 1;
        else if (cc > 64)
            cc = 64;

        if (cpd < 1)
            cpd = 1;
        else if (cpd > 20)
            cpd = 20;

        float min = 15f;
        float max = 140f;

        return new UserData()
        {
            cig_per_day = cpd,
            cig_count = cc,
            cig_price = Int16.Parse(_questions[2].InputText),

            currency = _questions[3].InputText,

            interval = (int)(TimeSpan.FromMinutes(max - (cpd - 1f) * (max - min) / 19f).TotalSeconds)
        };
    }

    private InputViewData[] _questions = [
        new InputViewData("How many cigarettes do you smoke per day?", "10", "You need to specify a number", "These data are needed to display your statistics\n1 - 20") { Keyboard = Keyboard.Numeric},
        new InputViewData("How many cigarettes are in one pack?", "20", "You need to specify a number", "These data are needed to display your statistics\n1 - 64") { Keyboard = Keyboard.Numeric},
        new InputViewData("How much does one pack of cigarettes cost?", "12", "You need to specify a number") { Keyboard = Keyboard.Numeric},
        new InputViewData("What is your currency?", "₽", "", "You can specify no more than three characters.\nFor example, \"$\" or \"USD\".") { InputLength = 3 },
    ];

    const uint ANIM_TIME = 350;
    const double ANIM_X = 400;

    private bool _onAnimation = false;
    private Border _border;
    private Label _label1, _label2;
    private Entry _textbox1;
    private Button _btnPrev, _btnNext;
    private int _curIndex;

}

public class InputViewData
{
    public InputViewData(string quetsion, string displayText, string placeHolder, string? desc = "These data are needed to display your statistics")
    {
        this.Question = quetsion;
        this.InputText = displayText;
        this.Placeholder = placeHolder;
        this.Description = desc;

        this.InputLength = 20;
        this.Keyboard = Keyboard.Default;
    }

    public string Question { get; set; }
    public string InputText { get; set; }
    public string Placeholder { get; set; }
    public string? Description { get; set; }

    public int InputLength { get; set; }
    public Keyboard Keyboard { get; set; }
}