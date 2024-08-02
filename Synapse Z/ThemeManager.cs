using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Synapse_Z;

public class ThemeManager
{
    private const string ThemeFolder = "theme";
    private const string ThemeFile = "theme.json";
    private const string EditorHtmlFileName = "editor.html";
    private JObject _themeSettings;

    private static ThemeManager _instance;

    public static ThemeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ThemeManager();
            }
            return _instance;
        }
    }

    private ThemeManager()
    {
        LoadOrCreateTheme();
    }

    private void LoadOrCreateTheme()
    {
        if (!Directory.Exists(ThemeFolder))
        {
            Directory.CreateDirectory(ThemeFolder);
        }

        string filePath = Path.Combine(ThemeFolder, ThemeFile);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            _themeSettings = JObject.Parse(json);
        }
        else
        {
            _themeSettings = GetDefaultThemeSettings();
            File.WriteAllText(filePath, _themeSettings.ToString());
        }

        // Check for the presence of an editor.html file and set the GlobalVariables.CurrentEditorHTML if found
        string editorFilePath = Path.Combine(ThemeFolder, "editor.html");
        if (File.Exists(editorFilePath))
        {
            GlobalVariables.CurrentEditorHTML = editorFilePath;
        }

        // Set the CurrentEditorTheme to the first item in Editor Themes
        var editorThemes = _themeSettings["Editor Themes"] as JArray;
        if (editorThemes != null && editorThemes.Count > 0)
        {
            GlobalVariables.CurrentEditorTheme = editorThemes[0].ToString();
        }

        // Set the ExploitName and ShowVersion global variables
        GlobalVariables.ExploitName = _themeSettings["ExploitName"]?.ToString() ?? "Synapse Z";
        GlobalVariables.ShowVersion = _themeSettings["ShowVersion"]?.ToObject<bool>() ?? false;
    }

    private JObject GetDefaultThemeSettings()
    {
        return new JObject
        {
            ["ExploitName"] = "Synapse Z",
            ["ShowVersion"] = true,
            ["Editor Themes"] = new JArray
            {
            },
            ["SynapseZ"] = new JObject
            {
                ["BackColor"] = "51, 51, 51, 255",
                ["BackgroundImageUrl"] = "",
                ["TopBar"] = new JObject
                {
                    ["BackColor"] = "61, 61, 61, 255"
                },
                ["synlabel"] = new JObject
                {
                    ["ForeColor"] = "255, 255, 255"
                },
                ["Panel"] = new JObject
                {
                    ["BackColor"] = "61, 61, 61, 255"
                },
                ["Button"] = new JObject
                {
                    ["BackColor"] = "61, 61, 61, 255",
                    ["HoverColor"] = "44, 62, 78, 255",
                    ["ClickColor"] = "54, 72, 88, 255",
                    ["ForeColor"] = "255, 255, 255"
                },
                ["ListBox"] = new JObject
                {
                    ["BackColor"] = "61, 61, 61, 255",
                    ["ForeColor"] = "255, 255, 255"
                },
                ["TabControl"] = new JObject
                {
                    ["BackColor"] = "61, 61, 61, 255",
                    ["ForeColor"] = "255, 255, 255"
                },
                ["Label"] = new JObject
                {
                    ["ForeColor"] = "255, 255, 255"
                },
                ["Logo"] = new JObject
                {
                    ["BackgroundImageUrl"] = ""
                }
            },
            ["AccountKeyPrompt"] = new JObject
            {
                ["BackColor"] = "51, 51, 51, 255",
                ["BackgroundImageUrl"] = "",
                ["TopBar"] = new JObject
                {
                    ["BackColor"] = "61, 61, 61, 255"
                },
                ["synlabel"] = new JObject
                {
                    ["ForeColor"] = "255, 255, 255"
                },
                ["Panel"] = new JObject
                {
                    ["BackColor"] = "61, 61, 61, 255"
                },
                ["Button"] = new JObject
                {
                    ["BackColor"] = "61, 61, 61, 255",
                    ["HoverColor"] = "44, 62, 78, 255",
                    ["ClickColor"] = "54, 72, 88, 255",
                    ["ForeColor"] = "255, 255, 255"
                },
                ["TextBox"] = new JObject
                {
                    ["BackColor"] = "61, 61, 61, 255",
                    ["ForeColor"] = "255, 255, 255"
                },
                ["Label"] = new JObject
                {
                    ["ForeColor"] = "255, 255, 255"
                },
                ["Logo"] = new JObject
                {
                    ["BackgroundImageUrl"] = ""
                }
            },
            ["ScriptHub"] = new JObject
            {
                ["BackColor"] = "51, 51, 51, 255",
                ["BackgroundImageUrl"] = "",
                ["TopBar"] = new JObject
                {
                    ["BackColor"] = "61, 61, 61, 255"
                },
                ["synlabel"] = new JObject
                {
                    ["ForeColor"] = "255, 255, 255"
                },
                ["Panel"] = new JObject
                {
                    ["BackColor"] = "61, 61, 61, 255"
                },
                ["Button"] = new JObject
                {
                    ["BackColor"] = "61, 61, 61, 255",
                    ["HoverColor"] = "44, 62, 78, 255",
                    ["ClickColor"] = "54, 72, 88, 255",
                    ["ForeColor"] = "255, 255, 255"
                },
                ["ListBox"] = new JObject
                {
                    ["BackColor"] = "30, 30, 30, 255",
                    ["ForeColor"] = "255, 255, 255"
                },
                ["Label"] = new JObject
                {
                    ["ForeColor"] = "255, 255, 255"
                },
                ["RichTextBox"] = new JObject
                {
                    ["BackColor"] = "30, 30, 30, 255",
                    ["ForeColor"] = "255, 255, 255"
                },
                ["Logo"] = new JObject
                {
                    ["BackgroundImageUrl"] = ""
                }
            },
            ["Options"] = new JObject
            {
                ["BackColor"] = "51, 51, 51, 255",
                ["BackgroundImageUrl"] = "",
                ["TopBar"] = new JObject
                {
                    ["BackColor"] = "61, 61, 61, 255"
                },
                ["synlabel"] = new JObject
                {
                    ["ForeColor"] = "255, 255, 255"
                },
                ["Panel"] = new JObject
                {
                    ["BackColor"] = "30, 30, 30, 255"
                },
                ["mainPanel"] = new JObject
                {
                    ["BackColor"] = "30, 30, 30, 255"
                },
                ["Button"] = new JObject
                {
                    ["BackColor"] = "61, 61, 61, 255",
                    ["HoverColor"] = "44, 62, 78, 255",
                    ["ClickColor"] = "54, 72, 88, 255",
                    ["ForeColor"] = "255, 255, 255"
                },
                ["CheckBox"] = new JObject
                {
                    ["ForeColor"] = "255, 255, 255"
                },
                ["ComboBox"] = new JObject
                {
                    ["BackColor"] = "61, 61, 61, 255",
                    ["ForeColor"] = "255, 255, 255"
                },
                ["Label"] = new JObject
                {
                    ["ForeColor"] = "255, 255, 255"
                },
                ["Logo"] = new JObject
                {
                    ["BackgroundImageUrl"] = ""
                }
            },
            ["ClientManager"] = new JObject
            {
                ["BackColor"] = "51, 51, 51, 255",
                ["BackgroundImageUrl"] = "",
                ["TopBar"] = new JObject
                {
                    ["BackColor"] = "61, 61, 61, 255"
                },
                ["synlabel"] = new JObject
                {
                    ["ForeColor"] = "255, 255, 255"
                },
                ["Panel"] = new JObject
                {
                    ["BackColor"] = "51, 51, 51, 255"
                },
                ["mainPanel"] = new JObject
                {
                    ["BackColor"] = "30, 30, 30, 255"
                },
                ["CheckBox"] = new JObject
                {
                    ["ForeColor"] = "255, 255, 255"
                },
                ["Button"] = new JObject
                {
                    ["BackColor"] = "61, 61, 61, 255",
                    ["HoverColor"] = "44, 62, 78, 255",
                    ["ClickColor"] = "54, 72, 88, 255",
                    ["ForeColor"] = "255, 255, 255"
                },
                ["Label"] = new JObject
                {
                    ["ForeColor"] = "255, 255, 255"
                },
                ["RichTextBox"] = new JObject
                {
                    ["BackColor"] = "30, 30, 30, 255",
                    ["ForeColor"] = "255, 255, 255"
                },
                ["Logo"] = new JObject
                {
                    ["BackgroundImageUrl"] = ""
                }
            },
        };
    }

    private Color ParseColor(string colorString)
    {
        var parts = colorString.Split(',');
        if (parts.Length == 3)
        {
            // If alpha is not provided, default to 255 (fully opaque)
            return Color.FromArgb(255, int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        }
        else if (parts.Length == 4)
        {
            return Color.FromArgb(int.Parse(parts[3]), int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        }
        else
        {
            throw new ArgumentException("Color string must have 3 or 4 components: R, G, B[, A].");
        }
    }

    public Color GetThemeColor(string path)
    {
        var colorString = _themeSettings.SelectToken(path)?.ToString();
        if (colorString != null)
        {
            return ParseColor(colorString);
        }

        throw new ArgumentException($"No color setting found for path: {path}");
    }

    public JArray GetEditorThemes()
    {
        return _themeSettings["Editor Themes"] as JArray;
    }

    private void CheckAndSetEditorHtml()
    {
        string editorHtmlPath = Path.Combine(ThemeFolder, EditorHtmlFileName);

        if (File.Exists(editorHtmlPath))
        {
            GlobalVariables.CurrentEditorHTML = File.ReadAllText(editorHtmlPath);
        }
        else
        {
            GlobalVariables.CurrentEditorHTML = null;
        }
    }

    public async void ApplyTheme(Control parent)
    {
        if (_themeSettings.TryGetValue(parent.Name, out JToken formSettings))
        {
            if (parent is Form form)
            {
                form.BackColor = ParseColor(formSettings["BackColor"].ToString());
                var backgroundImageUrl = formSettings["BackgroundImageUrl"].ToString();
                if (!string.IsNullOrEmpty(backgroundImageUrl))
                {
                    await SetBackgroundImage(form, backgroundImageUrl);
                }
            }

            ApplyControlTheme(parent, formSettings);
        }

        CheckAndSetEditorHtml();
    }

    private void ApplyControlTheme(Control control, JToken formSettings)
    {
        if (control.Name == "TopBar")
        {
            ApplyTopBarTheme((Panel)control, formSettings);
        }
        else if (control.Name == "synlabel")
        {
            ApplySynlabelTheme((Label)control, formSettings);
        }
        else if (control.Name == "mainPanel")
        {
            ApplyMainPanelTheme((Panel)control, formSettings);
        }
        else
        {
            switch (control)
            {
                case Panel panel:
                    ApplyPanelTheme(panel, formSettings);
                    break;
                case Button button:
                    ApplyButtonTheme(button, formSettings);
                    break;
                case ListBox listBox:
                    ApplyListBoxTheme(listBox, formSettings);
                    break;
                case Label label:
                    ApplyLabelTheme(label, formSettings);
                    break;
                case TextBox textBox:
                    ApplyTextBoxTheme(textBox, formSettings);
                    break;
                case PictureBox pictureBox when pictureBox.Name == "Logo":
                    ApplyLogoTheme(pictureBox, formSettings);
                    break;
                case TabControl tabControl:
                    ApplyTabControlTheme(tabControl, formSettings);
                    break;
                case CheckBox checkBox:
                    ApplyCheckBoxTheme(checkBox, formSettings);
                    break;
                case ComboBox comboBox:
                    ApplyComboBoxTheme(comboBox, formSettings);
                    break;
                case RichTextBox richTextBox:
                    ApplyRichTextBoxTheme(richTextBox, formSettings);
                    break;
            }
        }

        // Apply theme to nested controls
        foreach (Control nestedControl in control.Controls)
        {
            ApplyControlTheme(nestedControl, formSettings);
        }
    }

    private void ApplyPanelTheme(Panel panel, JToken formSettings)
    {
        var color = ParseColor(formSettings["Panel"]["BackColor"].ToString());
        panel.BackColor = color.A == 255 ? color : Color.Transparent;
    }

    private void ApplyMainPanelTheme(Panel mainPanel, JToken formSettings)
    {
        var color = ParseColor(formSettings["mainPanel"]["BackColor"].ToString());
        mainPanel.BackColor = color.A == 255 ? color : Color.Transparent;
    }

    private void ApplyButtonTheme(Button button, JToken formSettings)
    {
        var buttonSettings = formSettings["Button"];
        var color = ParseColor(buttonSettings["BackColor"].ToString());
        button.BackColor = color.A == 255 ? color : Color.Transparent;
        button.ForeColor = ParseColor(buttonSettings["ForeColor"].ToString());
        button.FlatAppearance.MouseOverBackColor = ParseColor(buttonSettings["HoverColor"].ToString());
        button.FlatAppearance.MouseDownBackColor = ParseColor(buttonSettings["ClickColor"].ToString());
    }

    private void ApplyListBoxTheme(ListBox listBox, JToken formSettings)
    {
        var color = ParseColor(formSettings["ListBox"]["BackColor"].ToString());
        listBox.BackColor = color.A == 255 ? color : Color.Transparent;
        listBox.ForeColor = ParseColor(formSettings["ListBox"]["ForeColor"].ToString());
    }

    private void ApplyLabelTheme(Label label, JToken formSettings)
    {
        label.ForeColor = ParseColor(formSettings["Label"]["ForeColor"].ToString());
    }

    private void ApplySynlabelTheme(Label label, JToken formSettings)
    {
        label.ForeColor = ParseColor(formSettings["synlabel"]["ForeColor"].ToString());
    }

    private void ApplyTextBoxTheme(TextBox textBox, JToken formSettings)
    {
        var color = ParseColor(formSettings["TextBox"]["BackColor"].ToString());
        textBox.BackColor = color.A == 255 ? color : Color.Transparent;
        textBox.ForeColor = ParseColor(formSettings["TextBox"]["ForeColor"].ToString());
    }

    private async void ApplyLogoTheme(PictureBox pictureBox, JToken formSettings)
    {
        var backgroundImageUrl = formSettings["Logo"]["BackgroundImageUrl"].ToString();
        if (!string.IsNullOrEmpty(backgroundImageUrl))
        {
            await SetBackgroundImage(pictureBox, backgroundImageUrl);
        }
    }

    private void ApplyTabControlTheme(TabControl tabControl, JToken formSettings)
    {
        var tabControlSettings = formSettings["TabControl"];
        var backColor = ParseColor(tabControlSettings["BackColor"].ToString());
        var foreColor = ParseColor(tabControlSettings["ForeColor"].ToString());

        tabControl.BackColor = backColor.A == 255 ? backColor : Color.Transparent;
        tabControl.ForeColor = foreColor;

        foreach (TabPage tabPage in tabControl.TabPages)
        {
            tabPage.BackColor = backColor;
            tabPage.ForeColor = foreColor;
            ApplyControlTheme(tabPage, formSettings);

            foreach (Control control in tabPage.Controls)
            {
                ApplyControlTheme(control, formSettings);
            }
        }
    }

    private void ApplyCheckBoxTheme(CheckBox checkBox, JToken formSettings)
    {
        var checkBoxSettings = formSettings["CheckBox"];
        if (checkBoxSettings != null)
        {
            var colorString = checkBoxSettings["ForeColor"]?.ToString();
            if (!string.IsNullOrEmpty(colorString))
            {
                checkBox.ForeColor = ParseColor(colorString);
            }
        }
        else
        {
            // Handle the case where CheckBox settings are not found
            checkBox.ForeColor = SystemColors.ControlText; // Default color
        }
    }


    private void ApplyComboBoxTheme(ComboBox comboBox, JToken formSettings)
    {
        var color = ParseColor(formSettings["ComboBox"]["BackColor"].ToString());
        comboBox.BackColor = color.A == 255 ? color : Color.Transparent;
        comboBox.ForeColor = ParseColor(formSettings["ComboBox"]["ForeColor"].ToString());
    }

    private void ApplyRichTextBoxTheme(RichTextBox richTextBox, JToken formSettings)
    {
        var color = ParseColor(formSettings["RichTextBox"]["BackColor"].ToString());
        richTextBox.BackColor = color.A == 255 ? color : Color.Transparent;
        richTextBox.ForeColor = ParseColor(formSettings["RichTextBox"]["ForeColor"].ToString());
    }

    private void ApplyTopBarTheme(Panel panel, JToken formSettings)
    {
        var color = ParseColor(formSettings["TopBar"]["BackColor"].ToString());
        panel.BackColor = color.A == 255 ? color : Color.Transparent;
    }

    private async Task SetBackgroundImage(Control control, string imageUrl)
    {
        string tempFilePath = null;
        string finalPath = null;

        try
        {
            if (File.Exists(imageUrl))
            {
                using (var image = new Bitmap(imageUrl))
                {
                    control.BackgroundImage = new Bitmap(image);
                    control.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            else if (Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(imageUrl);
                    response.EnsureSuccessStatusCode();

                    var tempFileName = $"{Guid.NewGuid()}.tmp";
                    tempFilePath = Path.Combine(Path.GetTempPath(), tempFileName);

                    await using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(fileStream);
                    }

                    // Convert webp to png if necessary
                    using (var image = Image.FromFile(tempFilePath))
                    {
                        finalPath = Path.ChangeExtension(tempFilePath, ".png");
                        image.Save(finalPath, System.Drawing.Imaging.ImageFormat.Png);

                        using (var finalImage = new Bitmap(finalPath))
                        {
                            control.BackgroundImage = new Bitmap(finalImage);
                            control.BackgroundImageLayout = ImageLayout.Stretch;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show($"Background image file '{imageUrl}' not found and is not a valid URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show($"Failed to set background image: Invalid image parameter. {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to set background image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (tempFilePath != null && File.Exists(tempFilePath))
            {
                try
                {
                    File.Delete(tempFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete temporary file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (finalPath != null && File.Exists(finalPath))
            {
                try
                {
                    File.Delete(finalPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete final file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
