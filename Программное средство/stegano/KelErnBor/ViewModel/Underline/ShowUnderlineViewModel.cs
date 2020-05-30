﻿using System;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stegano.Algorithm;
using Stegano.Model;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using Stegano.Interfaces;
using Stegano.Algorithm.Aditional_Coding;
using Stegano.Model.Font;
using Stegano.Model.Underline;

namespace Stegano.ViewModel.Underline
{
    public class ShowUnderlineViewModel : BaseShowViewModel
    {     
        public ShowUnderlineViewModel()
        {          
            DecodeUIInit();

            openFileDialog = new OpenFileDialog();

            RelayInit();
        }

        private void RelayInit()
        {            
            OpenForDecodeRelayCommand = new RelayCommand(OpenForDecode);

        }

        private void DecodeUIInit()
        {            
            AdditionalBitsCheckBox = new CheckBoxModel(true, false);
        }       

       
        private async void OpenForDecode()
        {
            try
            {
                if (string.IsNullOrEmpty(PathToDoc))
                {
                    ShowMetroMessageBox("Информация", "Загрузите файл для извлечения");
                    return;
                }

                CryptedText = string.Empty;
                SearchedText = string.Empty;
                TimeForDerypting = string.Empty;

                Stopwatch.Start();
                ShowUnderlineModel codeModel = new ShowUnderlineModel(PathToDoc);
                string foundedBitsInDoc = await codeModel.FindInformation();



                SearchedText = SelectedCodMethod == null ? SearchedText = Converter.BinaryToString(foundedBitsInDoc) : foundedBitsInDoc;
               
                if (SelectedHashMethod != null)
                {
                    if (string.IsNullOrEmpty(HashFile))
                    {
                        ShowMetroMessageBox("Информация", "Нет файла с хэшем!");
                        return;
                    }

                    var isHashSame = SelectedHashMethod.VerifyHash(SearchedText, HashFile);

                    if (!isHashSame)
                    {
                        ShowMetroMessageBox("Информация", "Не валидный хэш!");
                        return;
                    }
                }

                if (SelectedCodMethod != null)
                {
                    SearchedText = Converter.BinaryToString(SelectedCodMethod.DeCoding(SearchedText));
                }

                if (SelectedCryptMethod != null)
                {
                    if (string.IsNullOrEmpty(CryptFile))
                    {
                        ShowMetroMessageBox("Информация", "Нет файла с приватным ключом!");
                        return;
                    }

                    SearchedText = SelectedCryptMethod?.Decrypt(SearchedText, CryptFile) ?? SearchedText;
                  

                    if (string.IsNullOrEmpty(SearchedText))
                    {
                        ShowMetroMessageBox("Информация", "Ключ не подходит.");
                        return;
                    }


                }

                if (SearchedText.Length > 0)
                {
                    ShowMetroMessageBox("Информация", "Извлечение информации из файла " + openFileDialog.SafeFileName + " прошло успешно.");
                }
                else
                    ShowMetroMessageBox("Информация", "Файл " + openFileDialog.SafeFileName + " не содержит скрытой информации.");

                Stopwatch.Stop();
                TimeForDerypting = Math.Round(Stopwatch.Elapsed.TotalSeconds, 2).ToString() + " сек.";

                

            }
            catch (Exception e)
            {
                ShowMetroMessageBox("Информация", e.Message + "\n " + e.InnerException + "\n" + "\n" + e.Source);
            }
        }
       




        private OpenFileDialog OpenFileDialog(OpenFileDialog openFileDialog)
        {
            try
            {
                openFileDialog.Filter = "Все файлы|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    return openFileDialog;
                }

                return null;
            }
            catch (Exception exception)
            {
                ShowMetroMessageBox("Error", exception.Message);
            }

            return null;
        }

        private void ShowMetroMessageBox(string title, string message)
        {
            var mm = new ModernDialog
            {
                Title = title,
                Content = message
            };

            mm.ShowDialog();
        }
    }
}
