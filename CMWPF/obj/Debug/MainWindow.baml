   M S B A M L   `   `   `  ����@  <CMWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:CMWPFCMWPF  5   	   G CWpfScheduler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:WpfScheduler;assembly=WpfSchedulerWpfScheduler 5   	   7 clr-namespace:CMWPF.UserControlsCMWPF.UserControls  5	   	       CMWPF.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:CMWPF     5   	   X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f47toolkit)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   :sch0clr-namespace:WpfScheduler;assembly=WpfScheduler  5   	   2usercontrol clr-namespace:CMWPF.UserControls  5	   	   -   5          � Title$  
MainWindow��5   	   $	��720��6   $
��1280��6)    � WindowStyle$
 None=�66    � AllowsTransparency .  � WindowState$ 	Maximized=�6�   ��6c   Z� .��5      (� .��5   
   � $	��720��6   $
��1280��6   .+�5      �� -   5         ��Name$ 
TabControl��5      $��0q�6-   $��0q�6A   .@�5      ��  
  MyInfo#���   6   $
��Left=�6<   $	��Top=�6W     TabItemStyle#���  6o     	BrushBleu#���  6�   $	��238��6�   .��5      #   CMWPF.UserControls.MyProfilUC  -   5          Name$ 
MyProfilUC��5   -   5      �� 5        	Scheduler#���  6   #���  6@     
BrushGreen#���  6g   $	��190��6�   .��5      &    CMWPF.UserControls.MySchedulerUC  -   5          Name$ MySchedulerUC��5   0   5      �� 5        Care#���  6   $
��Left=�6:   $	��Top=�6U   #���  6m     BrushRed#���  6�   $	��211��6�   .��5      !   CMWPF.UserControls.MyCareUC  -   5          Name$ MyCareUC��5   +   5      �� 5        ToDo#���  6   $
��Left=�6:   #���  6U     BrushOrange#���  6|   $	��136��6�   .��5         CMWPF.UserControls.ToDoUC  -   5          Name$ ToDoUC��5   )   5      �� 5       		 Board#��� 	 6   $
��Left=�6;   #���  6V    
 BrushYellow#��� 
 6}   $	��151��6�   .��5      $   CMWPF.UserControls.NewsPaperUC  -   5        	  Name$	 NewsPaperUC��5   .   5      �� 5        	Transport#���  6   $
��Left=�6?   #���  6Z     	BrushGray#���  6�   �� 5        	MyArchive#���  6   $
��Left=�6?   #���  6Z     BrushViolet#���  6�     BrushWhiteForm#���  6�   $	��167��6�   .��5       "   CMWPF.UserControls.MyBoardUC  -   5        
  Name$
 	MyBoardUC��5    ,   5!      5"      �� 5#      -	   5        $��50��5#      $��80��6    $��Right=�6,   $	��Top=�6H   $y�	Collapsed=�6w   6�   $   CMWPF.UserControls.UserLoginUC  5$      -
   5          Name$ loggerUC��5$   &   $   CMWPF.UserControls.UserLoginUC   IsVisibleChanged$" UserLoginUC_IsVisibleChanged��68   � 5%      $��Bottom=�6   $	��155��6.   $y�	Collapsed=�6;    � RowDefinitions 5&      �� 5'      $��* �6$   �� 5(      $��30 �6$   �� 5)      $��25 �6$    � ColumnDefinitions 5+      �� 5,      $	��120 �6'   �� 5-      $	��120 �6'   �� 5.      $	��120 �6'   �� 5/      $��* �6'   .+�51      �� -   5        $��TextBox1��51      $��0��6+   $��0��68   $��4��6H    �� TextWrapping$
 Wrap=�6]   �� 52      -   5        $��LabelHypothesized��52      $��1��61   $��0��6>   
��� � �.��6b   Hypothesized6p   �� 53      -   5        $��LabelRecognized��53      $��1��6/   $��1��6<   
��� � �.��6`   
Recognized6l   �� 54      -   5        $��ButtonStart��54      $��1��6,   $��3��69   $��Start��6I   $��80��6s   {�.  6�   �� 55      -   5        $��LabelStatus��55      $��2��6+   $��0��68   $��10(�6H   
���  ��.��6g   	Status:6p   �� 56      -   5        $��Label1��56      $��2��6&   $��3��63   $��10(�6C   .��6Q   Speak "End Dictate" to stop.6o   57      �� 58      -   5        $��50��58   /   $��50��6:   $��Bottom=�6F   �� 59      -   5         ��Name$ image��59   +   $y�	Collapsed=�6   $��Images/scqsc.png��6:   $��Right=�6T   $��
0,0,-223,0q�6p   5:      5;      5<      