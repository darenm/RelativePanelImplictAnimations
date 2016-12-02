using System;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RelativePanelImplicitAnimations
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ImplicitAnimationCollection _implicitAnimations;

        public MainPage()
        {
            Timeline.AllowDependentAnimations = true;
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }


        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            EnsureImplicitAnimations();
            var elementVisual = ElementCompositionPreview.GetElementVisual(CommentsScrollviewer);
            elementVisual.ImplicitAnimations = _implicitAnimations;
            Poster.Visual.ImplicitAnimations = _implicitAnimations;
        }

        private void EnsureImplicitAnimations()
        {
            if (_implicitAnimations == null)
            {
                var compositor =
                    ElementCompositionPreview.GetElementVisual(this).Compositor;

                var offsetAnimation = compositor.CreateVector3KeyFrameAnimation();
                offsetAnimation.Target = nameof(Visual.Offset);
                offsetAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue");
                offsetAnimation.Duration = TimeSpan.FromMilliseconds(200);

                var sizeAnimation = compositor.CreateVector2KeyFrameAnimation();
                sizeAnimation.Target = nameof(Visual.Size);
                sizeAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue");
                sizeAnimation.Duration = TimeSpan.FromMilliseconds(200);

                _implicitAnimations = compositor.CreateImplicitAnimationCollection();
                _implicitAnimations[nameof(Visual.Offset)] = offsetAnimation;
                _implicitAnimations[nameof(Visual.Size)] = sizeAnimation;
            }
        }
    }
}