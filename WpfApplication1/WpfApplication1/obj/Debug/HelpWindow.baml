   M S B A M L   `   `   `  ����J  FWpfApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WpfApplication1WpfApplication1  5   	        WpfApplication1.HelpWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35I EWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=nullF 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:WpfApplication1     5   	     � Title$  
HelpWindow��5   	   $	��490��6   $	��550��6)   .��5
      ��  �� Padding$ 10q�6   ._�5   
   �  � ColumnDefinitions 5      �� 5      $
��270* �6#   .+�5      �� $��1��6   $��0 10 0 0q�6)   .+�5      �� $
��Bold"�6   $��18(�6*   .��68   #!Quick guide for using application6[   �� 5      -   5         ��Name$ 	StartInfo��5      $�� ��6/   $
��Wrap=�5      �� 5      -   5        $ HorizontalSplit��5      $;��5Horizontal Split - Split current window horizontally.��65   $
��Wrap=�6r   �� 5      -   5        $ VerticalSplit��5      $7��1Vertical Split - Split current window vertically.��63   $
��Wrap=�6l   �� 5      -   5        $ StockMonitoring��5      $B��<Stock monitoring - Monitoring the value of the stock market.��65   $
��Wrap=�6y   �� 5      -   5        $ CryptoMonitoring��5      $O��ICrypto currency monitoring - Monitoring the value of the crypto currency.��66   $
��Wrap=�6�   �� 5      -   5        $ ExChange��5      $/��)Foreign exchange - Converting currencies.��6.   $
��Wrap=�6_   �� 5      -   5        $ ClearWindow��5      $4��.Clear window - Delete current window contents.��61   $
��Wrap=�6g   �� 5      -   5        $
 Help��5      $)��#Help - Guide for using application.��6*   $
��Wrap=�6U   �� 5      -	   5        $ Refreshh��5      $5��/Refresh rate - Set the time for data retrieval.��6.   $
��Wrap=�6e   �� 5      -
   5        $ Description��5      $
��Wrap=�61   5!      5"      5#      5$      