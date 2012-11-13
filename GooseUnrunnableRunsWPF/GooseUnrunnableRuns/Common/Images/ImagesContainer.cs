using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Common
{
	public static class ImagesContainer
	{
		public static IList<ImageSource> GooseRunRightImages = new List<ImageSource>()
			{
				new BitmapImage(new Uri("pack://application:,,,/Common;component/Images/Goose_right_1.png")),
				new BitmapImage(new Uri("pack://application:,,,/Common;component/Images/Goose_right_2.png"))
			};

		public static IList<ImageSource> GooseRunLeftImages = new List<ImageSource>()
			{
				new BitmapImage(new Uri("pack://application:,,,/Common;component/Images/Goose_left_1.png")),
				new BitmapImage(new Uri("pack://application:,,,/Common;component/Images/Goose_left_2.png"))
			};
	}
}
