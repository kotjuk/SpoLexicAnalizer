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

        private void TreeButton_Click(object sender, EventArgs e)
        {
            string input = inputTextBox.Text;
            SyntaxParser parser = new SyntaxParser();
            try
            {
                TreeNode root = parser.Parse(input);
                syntaxTreeView.Nodes.Clear();
                syntaxTreeView.Nodes.Add(root);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка синтаксического дерева", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    public class LexicalAnalyzer
    {
        public string Analyze(string code)
        {
            // Используем HashSet для уникальных элементов
            HashSet<string> identifiers = new HashSet<string>();
            HashSet<string> numbers = new HashSet<string>();
            HashSet<string> keywords = new HashSet<string>();  // Храним ключевые сло-ва, которые встречаются в коде
            HashSet<string> ioOperators = new HashSet<string>();  // Операторы вво-да/вывода

            // Паттерн для нахождения токенов (ключевых слов, идентификаторов, чисел и т.д.)
            string pattern = @"int|printf|scanf|[a-zA-Z_][a-zA-Z0-9_]*|\d+|[();=+{}]";
            var matches = Regex.Matches(code, pattern);

            foreach (Match match in matches)
            {
                string token = match.Value;

                // Если это ключевое слово, добавляем в соответствующую коллекцию
                if (token == "int" || token == "printf" || token == "scanf")
                {
                    keywords.Add(token);
                }
                // Если это оператор ввода/вывода
                if (token == "printf" || token == "scanf")
                {
                    ioOperators.Add(token);
                }
                // Если это идентификатор (переменная)
                else if (Regex.IsMatch(token, @"[a-zA-Z_][a-zA-Z0-9_]*"))
                {
                    identifiers.Add(token);
                }
                // Если это число
                else if (int.TryParse(token, out _))
                {
                    numbers.Add(token);
                }
            }

            // Формируем строку с результатами анализа
            string result = "Идентификаторы: " + string.Join(", ", identifiers) + "\n";
            result += "Числа: " + string.Join(", ", numbers) + "\n";
            result += "Ключевые слова: " + string.Join(", ", keywords) + "\n";
            result += "Операторы ввода и вывода: " + string.Join(", ", ioOperators);

            return result;
        }
    }
    public class SyntaxParser
    {
        public TreeNode Parse(string code)
        {
            TreeNode root = new TreeNode("Программа");
            TreeNode mainNode = new TreeNode("int main ()");
            TreeNode blockNode = new TreeNode("{");
            TreeNode operatorListNode = new TreeNode("Список_операторов");

            string[] lines = code.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                // Убираем пробелы и переходим к разбору строк
                string trimmedLine = line.Trim();

                // Обработка присваивания
                if (trimmedLine.Contains("="))
                {
                    TreeNode assignmentNode = ParseAssignment(trimmedLine);
                    operatorListNode.Nodes.Add(assignmentNode);
                }
                // Обработка printf
                else if (trimmedLine.StartsWith("printf"))
                {
                    TreeNode printfNode = ParsePrintf(trimmedLine);
                    operatorListNode.Nodes.Add(printfNode);
                }
                // Обработка if
                else if (trimmedLine.StartsWith("if"))
                {
                    TreeNode ifNode = ParseIf(trimmedLine);
                    operatorListNode.Nodes.Add(ifNode);
                }
                // Можно добавить другие операторы здесь...
            }

            // Строим дерево
            root.Nodes.Add(mainNode);
            root.Nodes.Add(blockNode);
            blockNode.Nodes.Add(operatorListNode);
            blockNode.Nodes.Add(new TreeNode("}"));

            return root;
        }

        private TreeNode ParseAssignment(string line)
        {
            var parts = line.Split(new[] { '=' }, 2);
            var left = parts[0].Trim();
            var right = parts[1].Trim().TrimEnd(';');

            TreeNode assignmentNode = new TreeNode("Присваивание");
            assignmentNode.Nodes.Add(new TreeNode($"Идентификатор: {left}"));
            assignmentNode.Nodes.Add(new TreeNode("="));
            TreeNode rightExpressionNode = new TreeNode("Выражение");

            // Простейший случай, если правая часть - это число
            if (int.TryParse(right, out _))
            {
                rightExpressionNode.Nodes.Add(new TreeNode($"Число: {right}"));
            }
            else
            {
                rightExpressionNode.Nodes.Add(new TreeNode($"Слово: {right}"));
            }

            assignmentNode.Nodes.Add(rightExpressionNode);
            return assignmentNode;
        }

        private TreeNode ParsePrintf(string line)
        {
            var parts = line.Split(new[] { '(' }, 2);
            var arguments = parts[1].TrimEnd(')', ';').Split('+');

            TreeNode printfNode = new TreeNode("Вывод");
            printfNode.Nodes.Add(new TreeNode("printf"));

            TreeNode expressionNode = new TreeNode("Выражение");
            foreach (var argument in arguments)
            {
                expressionNode.Nodes.Add(new TreeNode($"Слово: {argument.Trim()}"));
            }

            printfNode.Nodes.Add(expressionNode);
            return printfNode;
        }

        private TreeNode ParseIf(string line)
        {
            var condition = line.Substring(2).Trim(); // Убираем 'if' из строки

            TreeNode ifNode = new TreeNode("Условие");
            ifNode.Nodes.Add(new TreeNode("if"));

            TreeNode conditionNode = new TreeNode("Условное_выражение");
            conditionNode.Nodes.Add(new TreeNode($"Слово: {condition}"));
            ifNode.Nodes.Add(conditionNode);

            // Для простоты добавим блоки с условием (псевдокод для блоков внутри if-else)
            ifNode.Nodes.Add(new TreeNode("{"));
            ifNode.Nodes.Add(new TreeNode("}"));
            return ifNode;
        }
    }



}

