﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.DataFormats;

namespace WinFormsApp1
{
    public partial class ChooseList : Form
    {
        private BindingList<string> _lists = new BindingList<string>(WordList.GetLists().ToList());
        public ChooseList()
        {
            InitializeComponent();
            directoryListBox.DataSource = _lists;
            directoryListBox.DoubleClick += new EventHandler(directoryListBox_MouseDoubleClick);
            directoryListBox.KeyPress += DirectoryListBox_KeyPress; 
            _lists.ListChanged += _lists_ListChanged;
        }

        private void _lists_ListChanged(object? sender, ListChangedEventArgs e)
        {
            directoryListBox_SelectedIndexChanged(sender, e);
        }

        private void DirectoryListBox_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == Convert.ToChar(Keys.Enter)) OpenList();
        }

        private void directoryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            deleteButton.Enabled = !string.IsNullOrEmpty(directoryListBox.Text);
            openButton.Enabled = !string.IsNullOrEmpty(directoryListBox.Text);
            if (deleteButton.Enabled)
            {
                WordList selected = WordList.LoadList(directoryListBox.Text);
                columnListBox.DataSource = selected.Languages;
                wordsCountLabel.Text = $"Words: {selected.Count}";
                label2.Text = $"Languages: {selected.Languages.Count()}";
            }
            else
            {
                columnListBox.DataSource = new List<string>();
                wordsCountLabel.Text = "Words: ";
                label2.Text = "Languages: ";
            }
        }

        private void directoryListBox_MouseDoubleClick(object sender, EventArgs e)
        {
            OpenList();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            Hide();
            CreateList form = new CreateList(_lists);
            form.Closed += (s, args) =>
            {
                Show();
            };
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            WordList.DeleteList(directoryListBox.Text);
            _lists.Remove(directoryListBox.Text);
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            OpenList();
        }

        private void OpenList()
        {
            Hide();
            DashBoard form = new DashBoard(WordList.LoadList(directoryListBox.Text));
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Closed += (s, args) => Show();
            form.Show();
        }
    }
}