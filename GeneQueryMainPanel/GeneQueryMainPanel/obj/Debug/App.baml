   M S B A M L   `   `   `  ����  GeneQueryMainPanel    GeneQueryMainPanel.App   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        �� 	Resources  5       �� MergedDictionaries �� Source   ButtonStyle1 �� 
TargetType � 	Resources  Key �Name J� Effect-  'System.Windows.Media.Effects.BlurEffect   Radius   RenderingBias'  !System.Windows.VisualStateManager	   VisualStateGroups%  System.Windows.VisualStateGroup
   States   System.Windows.VisualState �� Triggers :� RoutedEvent ��Name  
mouseEnter  
mouseLeave �� BeginStoryboardName  commonBtnStyle1 J� IsHitTestVisible  
gridHeader �� 
TargetType:  4System.Windows.Controls.Primitives.DataGridRowHeader �� Property �� Value�� 5       5      �� 5	   	   $ Simple Styles.xaml?�6   %o	  &	        7  7  &	 ~    &	 /    �� 5      " ��6-    5   	   ���� 5   
   
mouseEnter 6   �� 5   
   
mouseLeave 6   5   
   .R�5   	   � $
 grid��6   }�. 
������� 5   
     5      $ 3Q�6   $ Quality=�6!   	 5   
     5      CommonStates��.
 5        	MouseOver��6     5      Pressed��6     5      Disabled��6   5        5      FocusStates��.
 5        Focused��6   5        5      ValidationStates��.
 5        InvalidFocused��6     5       InvalidUnfocused��6   5!      5#   
    5$   	   :� 5%   
   $ Mouse.MouseEnter��6   .[�5&      �� $  mouseEnter_BeginStoryboard��6   8��  6?   5'      :� 5(   
   $ Mouse.MouseLeave��6   .[�5)      �� $  mouseLeave_BeginStoryboard��6   8�� 6?   �� 5*      $  mouseEnter_BeginStoryboard��6   5+      5-   	   �� 5.      " ��60   .R�5/   	   � ���	#FFE0E0E0 .  	 50   
     51      CommonStates��.
 52        	MouseOver��6     53      Pressed��6     54      Disabled��6   55        56      FocusStates��.
 57        Focused��6   58        59      ValidationStates��.
 5:        InvalidFocused��6     5;      InvalidUnfocused��6   5<      5>   
   5?   	   �� 5@      "  6'   .�5A      ��  � �� 5B      �� 5C      "  6*   .R�5D      � ���	#FFE0E0E0$��Center=�6:   	 5E   "     5F   &   CommonStates��.
 5G   *     Normal_CurrentRow��66     5H   *   Unfocused_EditingRow��66     5I   *   Normal_EditingRow��66     5J   *   	MouseOver��66     5K   *   MouseOver_CurrentRow��66     5L   *   "MouseOver_Unfocused_EditingRow��66     5M   *   MouseOver_EditingRow��66     5N   *    MouseOver_Unfocused_Selected��66     5O   *   MouseOver_Selected��66     5P   *   +'MouseOver_Unfocused_CurrentRow_Selected��66     5Q   *   !MouseOver_CurrentRow_Selected��66     5R   *   Unfocused_Selected��66     5S   *   !Unfocused_CurrentRow_Selected��66     5T   *   Normal_CurrentRow_Selected��66     5U   *   Normal_Selected��66   5V   '     5W   &   ValidationStates��.
 5X   *     InvalidFocused��66     5Y   *   InvalidUnfocused��66   5Z   '   5\      5]      5_      5`      5b      5e      