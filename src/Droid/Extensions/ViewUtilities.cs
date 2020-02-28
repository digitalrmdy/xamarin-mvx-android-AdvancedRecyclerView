using Android.Views;

namespace MvvmCross.AdvancedRecyclerView.Extensions
{
    public static class ViewUtils
    {
        public static bool HitTest(this View v, int x, int y)
        {
            var tx = (int) v.TranslationX + 0.5f;
            var ty = (int) v.TranslationY + 0.5f;
            var left = v.Left + tx;
            var right = v.Right + tx;
            var top = v.Top + ty;
            var bottom = v.Bottom + ty;

            return x >= left && x <= right && y >= top && y <= bottom;
        }
    }
}