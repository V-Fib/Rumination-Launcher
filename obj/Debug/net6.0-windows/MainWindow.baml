   M S B A M L   `   `   `  ����I  ESWG Rumination, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:SWG_RuminationSWG_Rumination  5   	       SWG_Rumination.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:SWG_Rumination     5   	     � Title$  SWG Rumination��5   	    � Icon$ /Resources/Icon.png��6    $	��675��5   	   $
��1200��6    � 
ResizeMode$ NoResize=�6#    � WindowStyle$
 None=�5	   	    � AllowsTransparency$
 True��6   
������  � WindowStartupLocation$ CenterScreen=�5
   	    � 	Resources 5         PrimaryColor �� Color  SecondaryColor��%d   &	        &	 (     �� 5   
   $ 	#FFDD9000��6/   6C   �� 5   
   $ 	#FF981414��61   6E   5      .��5      �� .+�5   
   �� -   5         �� Left$
 1150��5      
	 �� Top$	 5��6$   
 K� ZIndex$
 10��63   $��5,5,5,5q�6Z   $��0,0,0,0q�6l   $��30��6�   $��40                 ��6�   ��5      �� 5       �� 
TargetType" ��6   .�5      ��  �� Property � �� �� Value �	#9950505063   �� 5       �@�Cursor$
 Hand��6/   �� 5       �@��CornerRadius$ 0,4,4,0��6<   �� 5       � �� 5      �� 5       �� 
TargetType" ��6.   .R�5   "   ��  �� CornerRadius# z 6)   #��z��6^   ._�5   &   �� $��Center=�67   $��Center=�6T   5   #   5      5!       �� Triggers 5"      P� 5#       P� Property �@J�IsMouseOver P� Value$
 True��69   . �5$      ��  � �� �#C0392B6;   5%      5'      ��5)      �� 5*      $��/Resources/icon_close.png��6   $��13��6;   $��13                 ��6G    � Cursor$  ��5+      5-      �� 5/   
   -   5        $
 1110��5/      $	 5��6$   $
 10��63   $��5,5,5,5q�6^   $��0,0,0,0q�6p   $��30��6�   $��40                 ��6�   $
 Hand��50      ��51      �� 52      " ��6   .�53      ��  � �� �	#9950505063   �� 54       �@�Cursor$
 Hand��6/   �� 55       �@��CornerRadius$ 0,0,0,0��6<   �� 56       � �� 57      �� 58      " ��6.   .R�59   "   �� # z 6)   #��z��6^   ._�5:   &   �� $��Center=�67   $��Center=�6T   5;   #   5<      5>       5?      P� 5@       �@J�IsMouseOver$
 True��69   . �5A      ��  � �� �#5050506;   5B      5D      ��5F      �� 5G      $$��/Resources/icon_fullscreen.png��6   $��13��6@   $��13                 ��6L   5J      �� 5L   
   -   5        $
 1070��5L      $	 5��6$   $
 10��63   $��5,5,5,5q�6]   $��0,0,0,0q�6o   $��30��6�   $��40                 ��6�   $
 Hand��5M      ��5N      �� 5O      " ��6   .�5P      ��  � �� �	#9950505063   �� 5Q       �@�Cursor$
 Hand��6/   �� 5R       �@��CornerRadius$ 4,0,0,4��6<   �� 5S       � �� 5T      �� 5U      " ��6.   .R�5V   "   �� # z 6)   #��z��6^   ._�5W   &   �� $��Center=�67   $��Center=�6T   5X   #   5Y      5[       5\      P� 5]       �@J�IsMouseOver$
 True��69   . �5^      ��  � �� �#5050506;   5_      5a      ��5c      �� 5d      $"��/Resources/icon_minimize.png��6   $��13��6>   $��13                 ��6J   5g      �� 5j   
   $	��150��6   $��41��6   $ 30��6(   $	 0��69   $��/Resources/Logo_Nexus.png��6H   $
 10��6k   $��Center=�6}   $	��Top=�6�   �� 5m   
   -   5        $��43��5m      $
��1200��6    $ 0��6D   $	 0��6T   $
 9��6c   ��5n      �� 5o       �� EndPoint$ 1,0.5&�6&    �� 
StartPoint$ 0,0.5&�67   .��5p      �  � Color$ 	#BE000000��6#    � Offset$ 0.008Q�65   5q      5s      �� 5v   
   $	��675��6   $
��1200��6   $��/Resources/Background.jpg��6*   $��0,0,0,0q�6M   $ 0��6^   $	 0��6n   �� 5w   
   $	��650��6   $	��159��6   $"��/Resources/SWGRumination.png��6*   $��0,0,0,0q�6P   $ 0��6a    �� Bottom$ 0��6q   �� 5z   
   -   5        $��
PlayButton��5z      $��91��60   $	��326��6<    �� Right$ 20��6H   $ 40��6Z   $��0,0,0,0q�6m   
�������$��18(�6�   $��0,0,0,0q�6�   $  ��6�   $	 854��6�   $		 544��6�   ��5{      �� 5|      " ��6   .�5}      ��  � �� �	#00F0F8FF63   �� 5~       �@�Cursor$
 Hand��6/   �� 5       �@��CornerRadius$ 4,4,4,4��6<   �� 5�       � �� 5�      �� 5�      " ��6.   .R�5�   "   �� # z 6)   #��z��6^   ._�5�   &   �� $��Center=�67   $��Center=�6T   5�   #   5�      5�       5�      P� 5�       � {�$ False��67   . �5�      ��  � �� �#5050506;   5�      5�      ��5�      �� 5�      -   5        $��ButtonImage��5�      $#��/Resources/Button_AddGame.png��6+   5�      �� 5�   
   -   5        $��SettingsButton��5�   P    �� 	ClickMode$ Press=�6'   ���	#00F0FFFF$��50��6f   $��55��6r   $ 0��6}   $ 0��6�   $��10,10,10,10q�6�   $��0,0,0,0q�6�   ��5�      �� 5�      " ��6   .�5�      ��  � �� �#1ABC9C63   �� 5�       �@�Cursor$
 Hand��6/   �� 5�       �@��CornerRadius$ 0,4,4,0��6<   �� 5�       � �� 5�      �� 5�      " ��6.   .R�5�   "   �� # z 6)   #��z��6^   ._�5�   &   �� $��Center=�67   $��Center=�6T   5�   #   5�      5�       5�      P� 5�       � {�$ False��67   . �5�      ��  � �� �#5050506;   5�      P� 5�       �@J�IsMouseOver$
 True��69   . �5�      ��  � �� �#148E766;   5�      5�      ��5�      �� 5�       �� Orientation$ 
Horizontal=�6   .+�5�      �� $"��/Resources/icon_settings.png��6   $��20��6B   $��20                     ��6N   $��0,2,5,0q�5�      5�      5�      �� 5�   
   -   5        $��MenuSettings��5�      $	��200��6)   $	��250��66   $ 0��6B   $ 0��6T   $
 10��6d   .+�5�      �� $��	Game path��6   
�������6=   �� 5�      -	   5        $��PathName��5�   B   $	��...��67   $��5,0,5,5q�6R   $��20��6c   
���   �
�������6�   �� 5�      -
   5        $	��120��5�   (   $��0,20,0,0q�64   #���   6F   ��5�      �� 5�      $ 
Horizontal=�6!   .+�5�      �� -   5        $��ScanText��5�   G   
�������$��Normal"�63   .��6W   Check files6d   5�      5�      5�      �� 5�   
   -   5        $��BackgroundSettings��5�      $	��200��6.   $	��250��6;   $ 0��6G   $ 0��6Y   $
 9��6i   ��5�      �� 5�      $ 1,0.5&�6&   $ 0,0.5&�67   .��5�      � $ 	#E5000000��6#   5�      5�      �� 5�   
   $��Version 1.0.0��6   $
 1130��6(   $ 10��6;   ���#CCCCCC$��8(�6c   5�      5�      