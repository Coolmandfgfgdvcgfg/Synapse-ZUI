using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class CustomComboBoxHelper
{
    public static void ApplyCustomDrawing(ComboBox comboBox)
    {
        comboBox.DrawMode = DrawMode.OwnerDrawFixed;
        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBox.FlatStyle = FlatStyle.Flat;

        comboBox.DrawItem += new DrawItemEventHandler(CustomComboBox_DrawItem);
        comboBox.Paint += new PaintEventHandler(CustomComboBox_Paint);
        comboBox.HandleCreated += new EventHandler(CustomComboBox_HandleCreated);
        comboBox.Region = new Region(new Rectangle(3, 3, comboBox.Width - 3, comboBox.Height - 7));
    }

    private static void CustomComboBox_DrawItem(object sender, DrawItemEventArgs e)
    {
        ComboBox comboBox = sender as ComboBox;
        if (e.Index < 0) return;

        // Set the colors
        Color backColor = ThemeManager.Instance.GetThemeColor("Options.Button.BackColor");
        Color textColor = ThemeManager.Instance.GetThemeColor("Options.Button.ForeColor");
        Color hoverColor = ThemeManager.Instance.GetThemeColor("Options.Button.HoverColor");
        Color hoverBorderColor = Color.Gray;

        // Check if the item is selected or hovered
        if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
        {
            e.Graphics.FillRectangle(new SolidBrush(hoverColor), e.Bounds);
            e.Graphics.DrawRectangle(new Pen(hoverBorderColor, 1), e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
        }
        else
        {
            e.Graphics.FillRectangle(new SolidBrush(backColor), e.Bounds);
        }

        // Draw the text
        e.Graphics.DrawString(comboBox.Items[e.Index].ToString(), e.Font, new SolidBrush(textColor), new Point(e.Bounds.X, e.Bounds.Y));

        // Draw the focus rectangle
        e.DrawFocusRectangle();
    }

    private static void CustomComboBox_Paint(object sender, PaintEventArgs e)
    {
        ComboBox comboBox = sender as ComboBox;
        Rectangle rect = new Rectangle(0, 0, comboBox.Width, comboBox.Height);
        e.Graphics.FillRectangle(new SolidBrush(ThemeManager.Instance.GetThemeColor("ScriptHub.ComboBox.BackColor")), rect);
        if (comboBox.Text != string.Empty)
        {
            e.Graphics.DrawString(comboBox.Text, comboBox.Font, Brushes.White, new Point(1, 4));
        }
    }

    private static void CustomComboBox_HandleCreated(object sender, EventArgs e)
    {
        ComboBox comboBox = sender as ComboBox;
        int style = WinAPI.GetWindowLong(comboBox.Handle, WinAPI.GWL_EXSTYLE);
        style &= ~WinAPI.WS_EX_CLIENTEDGE;
        WinAPI.SetWindowLong(comboBox.Handle, WinAPI.GWL_EXSTYLE, style);
        WinAPI.SetWindowPos(comboBox.Handle, IntPtr.Zero, 0, 0, 0, 0, WinAPI.SWP_NOMOVE | WinAPI.SWP_NOSIZE | WinAPI.SWP_NOZORDER | WinAPI.SWP_FRAMECHANGED);
    }
}

internal static class WinAPI
{
    public const int GWL_EXSTYLE = -20;
    public const int WS_EX_CLIENTEDGE = 0x200;
    public const int SWP_NOMOVE = 0x0002;
    public const int SWP_NOSIZE = 0x0001;
    public const int SWP_NOZORDER = 0x0004;
    public const int SWP_FRAMECHANGED = 0x0020;

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
}
