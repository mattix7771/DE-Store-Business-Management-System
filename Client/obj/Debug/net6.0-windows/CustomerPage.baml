   M S B A M L   `   `   `  ����A  =Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ClientClient  5          Client.CustomerPage   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      &localclr-namespace:Client     5        N� Title$  CustomerPage��5       � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers P� Property	 P� Value��%|  '��       �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��5   !   #��z��5   !   $ 5��5   !   ._�5      �� $��Center=�6/   $��Center=�6L   5       5      P� 5       �@J�IsMouseOver$
	 True��6=   . �5   "   ��  � �� �#adadad6?   5      5      5      5      5      .��5!      � ���	#FFE5E5E5$	��450��6"   $	��800��6/   .+�5"   
   �� $��Create Product��6   $
��Left=�6)   $��	19,11,0,0q�6D   $	��Top=�6W   $��16(�6o   $
��Bold"�6}   �� 5#   
   $��Product name��6   $
��Left=�6'   $��	19,47,0,0q�6B   $	��Top=�6U   $
��Bold"�6m   �� 5$   
   -   5        
 ��Name$
 txt_productName��5$       �� TextWrapping$
 Wrap=�6+   $��22,73,644,347q�6?   �� 5%   
   $��Product amount��6   $
��Left=�6)   $��
19,129,0,0q�6D   $	��Top=�6X   $
��Bold"�6p   �� 5&   
   -   5        $
 txt_productAmount��5&      $
 Wrap=�6-   $��22,155,644,265q�6A   �� 5'   
   -   5         ��Name$ btn_purchase��5'      $��Purchase��6'   $
��Left=�6:   $��119,233,0,0q�6U   $	��Top=�6j   $��16(�6�   $
��Bold"�6�   $��34��6�   $��78��6�   6�   �� 5(   
   -   5         ��Name$ lbl_products��5(      $��Product List��6&   $
��Left=�6=   $��
315,27,0,0q�6X   $	��Top=�6l   $��16(�6�   $
��Bold"�6�   �� 5)   
   -   5        $ lbl_warning��5)      $�� ��6%   $
��Left=�60   $��
30,339,0,0q�6K   $	��Top=�6_   $	��256��6w   $��18(�6�   $
��Bold"�6�   $��Center=�6�   
���  ��$	��101��6�   �� 5*   
   -   5         ��Name$ productsList��5*      $��315,74,10,10q�6T    �� View 5+      �� 5,      .P�5-      �� 5.      50      �� 51   
   -   5         ��Name$ 
check_BNPL��51      $��Buy not pay later��6'   $
��Left=�6C   $��
89,198,0,0q�6^   $	��Top=�6r    d� 	IsChecked$ FalseY�6�    � UseLayoutRounding$ False��6�   $��RightToLeft=�6�   52      53      