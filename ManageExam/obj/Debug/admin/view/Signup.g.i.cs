﻿#pragma checksum "..\..\..\..\admin\view\Signup.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "EF672BC70E167A6B812C88288E1E1B9D6E3F1EBBDC0CAB876CE2034E2ACD538B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ManageExam.admin.view;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ManageExam.admin.view {
    
    
    /// <summary>
    /// Signup
    /// </summary>
    public partial class Signup : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 52 "..\..\..\..\admin\view\Signup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button screenMinimize;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\..\admin\view\Signup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button screenClose;
        
        #line default
        #line hidden
        
        
        #line 148 "..\..\..\..\admin\view\Signup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBox1;
        
        #line default
        #line hidden
        
        
        #line 172 "..\..\..\..\admin\view\Signup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBox2;
        
        #line default
        #line hidden
        
        
        #line 196 "..\..\..\..\admin\view\Signup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBox3;
        
        #line default
        #line hidden
        
        
        #line 213 "..\..\..\..\admin\view\Signup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button resgisterBtn;
        
        #line default
        #line hidden
        
        
        #line 240 "..\..\..\..\admin\view\Signup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.Hyperlink toLoginTab;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ManageExam;component/admin/view/signup.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\admin\view\Signup.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 14 "..\..\..\..\admin\view\Signup.xaml"
            ((ManageExam.admin.view.Signup)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.screenMinimize = ((System.Windows.Controls.Button)(target));
            
            #line 59 "..\..\..\..\admin\view\Signup.xaml"
            this.screenMinimize.Click += new System.Windows.RoutedEventHandler(this.screenMinimize_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.screenClose = ((System.Windows.Controls.Button)(target));
            
            #line 91 "..\..\..\..\admin\view\Signup.xaml"
            this.screenClose.Click += new System.Windows.RoutedEventHandler(this.screenClose_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.textBox1 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.textBox2 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.textBox3 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.resgisterBtn = ((System.Windows.Controls.Button)(target));
            return;
            case 8:
            this.toLoginTab = ((System.Windows.Documents.Hyperlink)(target));
            
            #line 241 "..\..\..\..\admin\view\Signup.xaml"
            this.toLoginTab.Click += new System.Windows.RoutedEventHandler(this.toLoginTab_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

