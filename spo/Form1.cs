using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace spo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void AnalyzeButton_Click(object sender, EventArgs e)
        {
            string input = inputTextBox.Text;
            LexicalAnalyzer analyzer = new LexicalAnalyzer();
            try
            {
                string result = analyzer.Analyze(input);
                MessageBox.Show(result, "Лексический и синтаксический анализ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка анализа", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private void TreeButton_Click(object sender, EventArgs e)
        //{
        //    string input = inputTextBox.Text;
        //    SyntaxParser parser = new SyntaxParser();
        //    try
        //    {
        //        TreeNode root = parser.Parse(input);
        //        syntaxTreeView.Nodes.Clear();
        //        syntaxTreeView.Nodes.Add(root);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Ошибка синтаксического дерева", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
    }

    public class LexicalAnalyzer
    {
        private enum TokenType { Identifier, Number, Keyword, Symbol }

        private static readonly HashSet<string> Keywords = new HashSet<string>
        {
            "int", "return", "printf", "scanf", "if", "else", "while", "for", "include"
        };

        public string Analyze(string code)
        {
            List<string> identifiers = new List<string>();
            List<string> numbers = new List<string>();
            List<string> keywords = new List<string>();
            List<string> symbols = new List<string>();

            int i = 0;
            while (i < code.Length)
            {
                char c = code[i];

                if (char.IsWhiteSpace(c))
                {
                    i++;
                    continue;
                }

                if (char.IsLetter(c) || c == '_')
                {
                    string identifier = ReadWhile(code, ref i, ch => char.IsLetterOrDigit(ch) || ch == '_');
                    if (Keywords.Contains(identifier))
                        keywords.Add(identifier);
                    else
                        identifiers.Add(identifier);
                }
                else if (char.IsDigit(c))
                {
                    string number = ReadWhile(code, ref i, ch => char.IsDigit(ch));
                    numbers.Add(number);
                }
                else
                {
                    symbols.Add(c.ToString());
                    i++;
                }
            }

            return $"Идентификаторы: {string.Join(", ", identifiers)}\n" +
                   $"Числа: {string.Join(", ", numbers)}\n" +
                   $"Ключевые слова: {string.Join(", ", keywords)}\n" +
                   $"Символы: {string.Join(" ", symbols)}";
        }

        private string ReadWhile(string code, ref int index, Func<char, bool> condition)
        {
            int start = index;
            while (index < code.Length && condition(code[index]))
            {
                index++;
            }
            return code.Substring(start, index - start);
        }
    }


    //public class SyntaxParser
    //{
    //    public TreeNode Parse(string code)
    //    {
    //        TreeNode root = new TreeNode("Программа");
    //        TreeNode mainNode = new TreeNode("int main ()");
    //        TreeNode blockNode = new TreeNode("{");
    //        TreeNode operatorListNode = new TreeNode("Список_операторов");

    //        string[] lines = code.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

    //        foreach (var line in lines)
    //        {
    //            // Убираем пробелы и переходим к разбору строк
    //            string trimmedLine = line.Trim();

    //            // Обработка присваивания
    //            if (trimmedLine.Contains("="))
    //            {
    //                TreeNode assignmentNode = ParseAssignment(trimmedLine);
    //                operatorListNode.Nodes.Add(assignmentNode);
    //            }
    //            // Обработка printf
    //            else if (trimmedLine.StartsWith("printf"))
    //            {
    //                TreeNode printfNode = ParsePrintf(trimmedLine);
    //                operatorListNode.Nodes.Add(printfNode);
    //            }
    //            // Обработка if
    //            else if (trimmedLine.StartsWith("if"))
    //            {
    //                TreeNode ifNode = ParseIf(trimmedLine);
    //                operatorListNode.Nodes.Add(ifNode);
    //            }
    //            // Можно добавить другие операторы здесь...
    //        }

    //        // Строим дерево
    //        root.Nodes.Add(mainNode);
    //        root.Nodes.Add(blockNode);
    //        blockNode.Nodes.Add(operatorListNode);
    //        blockNode.Nodes.Add(new TreeNode("}"));

    //        return root;
    //    }

    //    private TreeNode ParseAssignment(string line)
    //    {
    //        var parts = line.Split(new[] { '=' }, 2);
    //        var left = parts[0].Trim();
    //        var right = parts[1].Trim().TrimEnd(';');

    //        TreeNode assignmentNode = new TreeNode("Присваивание");
    //        assignmentNode.Nodes.Add(new TreeNode($"Идентификатор: {left}"));
    //        assignmentNode.Nodes.Add(new TreeNode("="));
    //        TreeNode rightExpressionNode = new TreeNode("Выражение");

    //        // Простейший случай, если правая часть - это число
    //        if (int.TryParse(right, out _))
    //        {
    //            rightExpressionNode.Nodes.Add(new TreeNode($"Число: {right}"));
    //        }
    //        else
    //        {
    //            rightExpressionNode.Nodes.Add(new TreeNode($"Слово: {right}"));
    //        }

    //        assignmentNode.Nodes.Add(rightExpressionNode);
    //        return assignmentNode;
    //    }

    //    private TreeNode ParsePrintf(string line)
    //    {
    //        var parts = line.Split(new[] { '(' }, 2);
    //        var arguments = parts[1].TrimEnd(')', ';').Split('+');

    //        TreeNode printfNode = new TreeNode("Вывод");
    //        printfNode.Nodes.Add(new TreeNode("printf"));

    //        TreeNode expressionNode = new TreeNode("Выражение");
    //        foreach (var argument in arguments)
    //        {
    //            expressionNode.Nodes.Add(new TreeNode($"Слово: {argument.Trim()}"));
    //        }

    //        printfNode.Nodes.Add(expressionNode);
    //        return printfNode;
    //    }

    //    private TreeNode ParseIf(string line)
    //    {
    //        var condition = line.Substring(2).Trim(); // Убираем 'if' из строки

    //        TreeNode ifNode = new TreeNode("Условие");
    //        ifNode.Nodes.Add(new TreeNode("if"));

    //        TreeNode conditionNode = new TreeNode("Условное_выражение");
    //        conditionNode.Nodes.Add(new TreeNode($"Слово: {condition}"));
    //        ifNode.Nodes.Add(conditionNode);

    //        // Для простоты добавим блоки с условием (псевдокод для блоков внутри if-else)
    //        ifNode.Nodes.Add(new TreeNode("{"));
    //        ifNode.Nodes.Add(new TreeNode("}"));
    //        return ifNode;
    //    }
    //}
}

