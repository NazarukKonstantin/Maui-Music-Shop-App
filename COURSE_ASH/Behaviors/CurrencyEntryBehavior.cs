using System.Linq;

namespace COURSE_ASH.Behaviors;

public class CurrencyEntryBehavior : Behavior<Entry>
{
    private bool _hasFormattedOnce = false;
    protected override void OnAttachedTo(Entry entry)
    {
        entry.TextChanged += OnEntryTextChanged;
        entry.Focused += EntryOnFocused;
        entry.Unfocused += EntryOnUnfocused;
        base.OnAttachedTo(entry);
    }

    private void EntryOnUnfocused(object sender, FocusEventArgs e)
    {
        var entry = sender as Entry;
        if (string.IsNullOrEmpty(entry?.Text))
        {
            entry.Text = "0.00";
        }
    }

    private void EntryOnFocused(object sender, FocusEventArgs e)
    {
        var entry = sender as Entry;
        if (entry?.Text == "0.00")
        {
            entry.Text = "";
        }
    }

    protected override void OnDetachingFrom(Entry entry)
    {
        entry.TextChanged -= OnEntryTextChanged;
        entry.Focused -= EntryOnFocused;
        entry.Unfocused -= EntryOnUnfocused;
        base.OnDetachingFrom(entry);
    }

    private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
    {
        var entry = sender as Entry;
        var oldText = args?.OldTextValue;
        var newText = args?.NewTextValue;
    
        if (!string.IsNullOrEmpty(newText)
            && !string.IsNullOrEmpty(oldText)
            && oldText.Contains('.')
            && (newText.Last() == '.'
            || (char.IsDigit(newText.Last())
            && oldText.Substring(oldText.LastIndexOf('.') + 1).Count() == 2)))
        {
            entry.Text = newText.Remove(newText.Length - 1, 1);
        }

        if (!_hasFormattedOnce && newText == "0")
        {
            entry.Text = "0.00";
            _hasFormattedOnce = true;
        }
    }
}
