﻿#pragma checksum "..\..\..\PageContent\Login.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "07005D5F8166541257AD83B6205D8150"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace GeneQueryMainPanel.PageContent {
    
    
    /// <summary>
    /// Window1
    /// </summary>
    public partial class Window1 : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 4 "..\..\..\PageContent\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal GeneQueryMainPanel.PageContent.Window1 MainPanel;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\PageContent\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label userNameLable;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\PageContent\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox passwordInput;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\PageContent\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\PageContent\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button login_cancelBtn;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\PageContent\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle elertBox;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\PageContent\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label userNameLable_Copy;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\PageContent\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox usernameInput;
        
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
            System.Uri resourceLocater = new System.Uri("/GeneQueryMainPanel;component/pagecontent/login.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\PageContent\Login.xaml"
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
            this.MainPanel = ((GeneQueryMainPanel.PageContent.Window1)(target));
            return;
            case 2:
            this.userNameLable = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.passwordInput = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 23 "..\..\..\PageContent\Login.xaml"
            this.passwordInput.GotFocus += new System.Windows.RoutedEventHandler(this.passwordInput_GotFocus);
            
            #line default
            #line hidden
            return;
            case 4:
            this.button = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\..\PageContent\Login.xaml"
            this.button.Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            case 5:
            this.login_cancelBtn = ((System.Windows.Controls.Button)(target));
            
            #line 33 "..\..\..\PageContent\Login.xaml"
            this.login_cancelBtn.Click += new System.Windows.RoutedEventHandler(this.login_cancelBtn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.elertBox = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 7:
            this.userNameLable_Copy = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.usernameInput = ((System.Windows.Controls.TextBox)(target));
            
            #line 42 "..\..\..\PageContent\Login.xaml"
            this.usernameInput.GotFocus += new System.Windows.RoutedEventHandler(this.usernameInput_GotFocus);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

