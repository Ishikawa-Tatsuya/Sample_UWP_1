using Microsoft.VisualStudio.TestTools.UnitTesting;
using Friendly.UWP;
using Codeer.Friendly.Dynamic;
using EnvDTE80;
using System.IO;
using VSHTC.Friendly.PinInterface;

namespace Sample
{
    [TestClass]
    public class Demo
    {
        [TestMethod]
        public void Test()
        {
            using (var app = new UWPAppFriend(new ByVisualStudio(Path.GetFullPath("../../../TargetApp/TargetApp.sln"))
            {
                VisualStudioPath = @"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe",
                ChangeVisualStudioSetting = (vs, dteSrc) =>
                {
                    var dte = dteSrc.Pin<DTE2>();
                    dte.Solution.SolutionBuild.SolutionConfigurations.Item(3).Activate();
                },
                ContinueDebuging = true
            }))
            {
                var current = app.Type("Windows.UI.Xaml.Window").Current;
                var mainPage = current.Content.Content;

                //invoke method.
                string ret = mainPage.Func(10);
                Assert.AreEqual("10", ret);

                //change color.
                var color = app.Type("Windows.UI.Colors").Blue;
                var brush = app.Type("Windows.UI.Xaml.Media.SolidColorBrush")(color);
                mainPage.Content.Background = brush;
            }
        }
    }
}
