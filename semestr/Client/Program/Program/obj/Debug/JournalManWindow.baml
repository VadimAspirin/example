   M S B A M L   `   `   `  ����B  >Program, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:ProgramProgram  5   	       Program.JournalManWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Program     5   	     � Title$#  Ведение журнала��5   	   $��454.913��6!   $��443.499��62    � 
ResizeMode$ NoResize=�6B   .��5	      � .+�5
   
   (  "System.Windows.Controls.DatePicker  -   5        $��datePickerComing��5
      $
��Left=�6-   $��
125,46,0,0q�6H   $	��Top=�6\   $	��300��6t   $��24��6�      DisplayDate$ 
2017-06-15��6�      SelectedDateFormat$ Short=�6�   �� 5   
   -   5        $��comboBoxWorker��5      $
��Left=�6)   $��
125,84,0,0q�6D   $	��Top=�6X   $	��300��6p   $��22��6|    �� DisplayMemberPath$ ФИО��6�    �� SelectedValuePath$  Идентификатор��6�   ��6�   �� 6�   �� 5   
   -   5        $��buttonOK��5      $��OK��6!   $
��Left=�6.   $��50��6I   $��313,119,0,0q�6U   $	��Top=�6j   $	��112��6�   6�   �� 5   
   $��Работник:��6   $
��Left=�6$   $��	56,80,0,0q�6?   $	��Top=�6R   �� 5   
   $$��Дата посещения:
��6   $
��Left=�64   $��	17,46,0,0q�6O   $	��Top=�6b    J� RenderTransformOrigin$ -6.454,-1.213&�6z   �� 5   
   $!��Добавить смену��6   $
��Left=�6)   $��	75,10,0,0q�6D   $	��Top=�6W   $ 0.255,0.162&�6o   &   System.Windows.Controls.DataGrid  5   
   -   5        $��dataGridVisits��5      $
��Left=�6)   $	��241��6D   $��
10,174,0,0q�6Q   $	��Top=�6e   $	��415��6}      
IsReadOnly .    SelectionMode$ Single=�6�   ��6�   �� 6�   �� 5   
   -   5        $��buttonUpdate��5      $
��Left=�6%   $��50��6@   $��
10,119,0,0q�6L   $	��Top=�6`   $��50��6x   .��5      � .+�5      �� $��0��6   $$��Resources/обновить.png��6%   5      5      5      5      