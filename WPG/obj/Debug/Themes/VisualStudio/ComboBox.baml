   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   �   I2clr-namespace:System.Collections;assembly=mscorlibSystem.Collections  6�   E AWPG, Version=2.0.5426.26629, Culture=neutral, PublicKeyToken=nullclr-namespace:WPGWPG 6  #clr-namespace:WPG.DataWPG.Data 6!  /clr-namespace:WPG.ConvertersWPG.Converters 6E  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   ComboBoxToggleButton  �� 
TargetType ��Name �� TypeInTargetAssembly   WPG.PropertyGrid �� 
ResourceId F�Name �� Triggers P� Property P� Value �� 
TargetName	 �� Value
 �� Property  ComboBoxTextBox ��Name  ComboBoxSelectedItem �� DataType �� Padding �� 
TargetType � ColumnDefinitions d�Name �� 	ClickMode d� 	IsChecked �� Path �� Mode �� RelativeSource ��Name �� SelectionBoxItem J� IsHitTestVisible ��Name �� 
IsReadOnly �� 
IsReadOnly �Name �� IsDropDownOpen � AllowsTransparency �Name  �� MaxDropDownHeight! � ActualWidth" J� SnapsToDevicePixels# K� IsItemsHost-  'System.Windows.Input.KeyboardNavigation$   DirectionalNavigation% P� 
SourceName  SelectedBackgroundBrush  DisabledForegroundBrush�� 6   D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     6W   .clr&clr-namespace:System;assembly=mscorlib  6�   ;clr22clr-namespace:System.Collections;assembly=mscorlib  6�   localclr-namespace:WPG  6  dataclr-namespace:WPG.Data  6!  +
convertersclr-namespace:WPG.Converters  6E  8d2http://schemas.microsoft.com/expression/blend/2006  6u  Bmc;http://schemas.openxmlformats.org/markup-compatibility/2006  6�  %�  &	        &	 |    &	 �    (�� E    5&   	   "   $ ComboBoxStyle��)7   7  7  '�� T    �� 5      "  d�60   .R�5      � .+�5      �� $ 	Rectangle��6-   $��Stretch=�6   $��Stretch=�6@   $
��Auto��6\   $
��Auto��6i   ��6w   C� *�� "   $ NormalBrush��+��6�   C� *�� "   $ NormalBorderBrush��+F� 5      $ Arrow��6'   $��Center=�6
   $��Center=�66   "����        � 	=  	= � z     	��6Q   C� *�� "   $ 
GlyphBrush��+6�   5	       5
      P� 5       �@J�IsMouseOver . . �5      �� $ 	Rectangle��6�   	 6   C� *�� "   $ MouseOverBrush��+
 � ��6�   5      P� 5       �@d�	IsChecked$
 trueY�6"   . �5      �� $ 	Rectangle��6�   	 6   C� *�� "   $ PressedBrush��+
 � ��6�   5      P� 5       � {� .  . �5      �� $ 	Rectangle��6�   	 6   C� *�� "   $ DisabledBackgroundBrush��+
 � ��6�   �� 5      $ 	Rectangle��6�   	 6   C� *�� "   $ DisabledBorderBrush��+
 � ��6�   �� 5      	 6#   C� *�� "   $ DisabledForegroundBrush��+
 � ���� 5      $ Arrow��6�   	 6   C� *�� "   $ DisabledForegroundBrush��+
 � ��6�   5      5      �� 5      "  ��6+   .R�5      �� $ PART_ContentHost��6   |�.  #��z��67   5      �� 5!      $ ComboBoxItem��6-   .R�5"      �� $ 0q�6   $��0q�6   ��6%   �� 5#      �� 5&      " ��6s   .�5'      �� 
 �@J�SnapsToDevicePixels	 . 6*   �� 5(      
 � ��	 5)      �� 5*      "  ��6   .R�5+      �  5,      �� 5-   	   6   �� 5.   	   $��20 �6   6'   .+�51      d� $ ToggleButton��6k   $y�Visible=�6   $��1��6*   8��  |�.  $ Press=�6�    6�   �� $ IsDropDownOpen�$ TwoWay=� �� *TemplatedParent+6�   �� 52      $ ContentSite��6U   $��0��6   $
��Left=�6)   $��2,0,0,0q�6D   $��Center=�6j   #��z 6�   8�� #��z��6�    .  6+  �� 55      $ PART_EditableTextBox��6�   $��0��6   
������ $y�	Collapsed=�6:   8�� $
��Left=�6}   $��3,3,23,3q�6�   $��Center=�6�   |�. 
������ # z 6  ��6�   Z� 6  � 58      $ Popup��6K   #��z 6   $��Bottom=�68   |�.   . $��Slide=�6�   .��59   	   � $ DropDown��6g   #��z  6   #��z! 6>   " . .+�5:   
   �� $ DropDownBorder��6   $��1q�67  ��6)   C� *�� "   $ WindowBackgroundBrush��+��6�   C� *�� "   $ SolidBorderBrush��+67  �� 5;   
   $��4,6,4,6q�6   " . $
��Auto=�6�   $
��Auto=�6�   ��. ��6(   C� *�� "   $ PropertyGridScrollViewer��+.��5>      �� # . $$ 	Contained=�6)   5@      5A   
   5B   	   5C       5D      P� 5F       �@J�IsMouseOver . . �5G   	   �� $ PART_EditableTextBox��6�   	 6'   C� *�� "   $ SolidBorderBrush��+
 � ��6�   �� 5H   	   $ ToggleButton��66   
 � y�$	 Visible=�6&   66   5I   	   P� 5J       �@J�	IsFocused . . �5K   	   �� $ PART_EditableTextBox��6�   	 6'   C� *�� "   $ SolidBorderBrush��+
 � ��6�   �� 5L   	   $ ToggleButton��66   
 � y�$	 Visible=�6&   66   5M   	   P� 5O       �@��HasItems .  . �5P   	   �� $ DropDownBorder��60   
 � ��$	 95��6%   60   5Q   	   P� 5R       � {� .  . �5S   	   �� 	 6&   C� *�� "   $ DisabledForegroundBrush��+
 � ��5T   	   P� 5U       �@��
IsGrouping . . �5V   	   �� 
 � ��	 .  69   5W   	   P� 5X      $% Popup��6.    �@�AllowsTransparency . . �5Y   	   �� $ DropDownBorder��62   
 �@��CornerRadius$	 4��6(   62   �� 5Z   	   $ DropDownBorder��62   
 � ��$	 0,2,0,0q�6"   62   5[   	   P� 5\       �@��
IsEditable . . �5]   	   �� 
 � ��	 .  6%   �� 5^   	   $ PART_EditableTextBox��66   
 � y�$	 Visible=�6&   66   �� 5_   	   $ ContentSite��68   
 � y�$	 	Collapsed=�6&   68   5`   	   5b      5d      5e      �� 5h      " ��6	   .�5i      �� 
 �@J�SnapsToDevicePixels	 . 6*   �� 5j      
 � ��	 5k      �� 5l      "  ��6   .R�5m      � " . .+�5n      �� $ Border��6   #��z��6   #��z��6I   #��z��6u   �� 5o      #��z��6   #��z��6\   5p       5q      P� 5t       �@��IsHighlighted . . �5u   	   �� #	 �  6&   $ Border��6X   
 � ��5v   	   P� 5x       � {� .  . �5y   	   �� #	 �  6&   
 � ��5z   	   5|      5~      5      5�      