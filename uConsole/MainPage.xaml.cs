﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace uConsole
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Negocio.clConsole Console = new Negocio.clConsole();
        public MainPage()
        {
            InitializeComponent();
            Console.Conectado = false;
            Console.Diretorio = "Agora o diretório é outro";
            DataContext = Console;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void btnNavega_Click(object sender, RoutedEventArgs e)
        {
            Button botao = (Button)sender;
            switch (botao.Name)
            {
                case "btnConnecta":
                    conteudo.Navigate(typeof(pgConexao), Console);
                    break;
                case "btnEnvia":
                    conteudo.Navigate(typeof(pgPublicacao), Console);
                    break;
                case "btnComando":
                    conteudo.Navigate(typeof(pgComandos), Console);
                    break;
            }

        }


    }

}