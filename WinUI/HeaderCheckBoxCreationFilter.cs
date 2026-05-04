using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
//
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace WinUI
{
    #region HeaderCheckBoxCreationFilter

    /// <summary>
    /// This Creation Filter class puts a CheckBoxUIElement in the header of each column whose DataType is Boolean.
    ///	It will fire the Clicked event whenever the CheckBox's CheckState changes.	
    /// </summary>
    public class HeaderCheckBoxCreationFilter : IUIElementCreationFilter
    {
        #region Data

        /// <summary>
        /// This event will fire when the CheckBox is clicked. 
        /// </summary> 
        public event HeaderCheckBoxClickedHandler CheckChanged;

        public delegate void HeaderCheckBoxClickedHandler(object sender, HeaderCheckBoxEventArgs e);

        /// <summary>
        /// Stores the CheckState for each boolean column in the WinGrid associated with this filter.
        /// Note, if one instance of this filter is used with multiple grids at the same time,
        /// be sure that the column keys of boolean columns in the grids are not identical.
        /// </summary>
        protected Hashtable hashCheckStates = new Hashtable();

        #endregion Data

        #region HeaderCheckBoxEventArgs

        // Event argument class used for the CheckChanged event. 
        public class HeaderCheckBoxEventArgs : EventArgs
        {
            #region Data

            private UltraGridColumn column;
            private CheckState checkState;

            #endregion Data

            #region Constructor

            public HeaderCheckBoxEventArgs(UltraGridColumn column, CheckState checkState)
            {
                this.column = column;
                this.checkState = checkState;
            }

            #endregion Constructor

            #region Column

            /// <summary>
            /// The UltraGridColumn whose header contains the CheckBoxUIElement that was clicked.
            /// </summary>
            public UltraGridColumn Column
            {
                get
                {
                    return this.column;
                }
            }

            #endregion Column

            #region CurrentCheckState

            /// <summary>
            /// The new CheckState of the CheckBoxUIElement that was clicked.
            /// </summary>
            public CheckState CurrentCheckState
            {
                get
                {
                    return this.checkState;
                }
            }

            #endregion CurrentCheckState
        }

        #endregion

        #region OnCheckBoxUIElementElementClick

        // This method is invoked when the user clicks on a check box in a column header.
        private void OnCheckBoxUIElementElementClick(object sender, Infragistics.Win.UIElementEventArgs e)
        {
            // Get the CheckBoxUIElement that was clicked
            CheckBoxUIElement checkBoxUIElement = e.Element as CheckBoxUIElement;

            // Get the ColumnHeader associated with this particular element
            HeaderUIElement headerUIElement =
                checkBoxUIElement.GetAncestor(typeof(HeaderUIElement))
                as HeaderUIElement;

            Infragistics.Win.UltraWinGrid.ColumnHeader columnHeader =
                headerUIElement.GetContext(typeof(Infragistics.Win.UltraWinGrid.ColumnHeader))
                as Infragistics.Win.UltraWinGrid.ColumnHeader;

            // Store the CheckState of this column header.
            this.hashCheckStates[columnHeader.Column.Key] = checkBoxUIElement.CheckState;

            // Raise the CheckChanged event so that the listener(s) know that the CheckState changed.
            if (this.CheckChanged != null)
                this.CheckChanged(this, new HeaderCheckBoxEventArgs(columnHeader.Column, checkBoxUIElement.CheckState));
        }

        #endregion OnCheckBoxUIElementElementClick

        #region IUIElementCreationFilter Implementation

        #region BeforeCreateChildElements

        public bool BeforeCreateChildElements(Infragistics.Win.UIElement parent)
        {
            // We do not want to do anything before the child elements of 
            // the column header are created, so just return false.
            return false;
        }

        #endregion BeforeCreateChildElements

        #region AfterCreateChildElements

        public void AfterCreateChildElements(Infragistics.Win.UIElement parent)
        {
            // Check if the element that was just created is a HeaderUIElement.
            // We are going to put each CheckBoxUIElement on a HeaderUIElement.
            if (parent is HeaderUIElement)
            {
                // Get the HeaderBase object from the HeaderUIElement
                Infragistics.Win.UltraWinGrid.HeaderBase header = (parent as HeaderUIElement).Header;

                // Only put the checkbox into headers whose DataType is Boolean
                if (header.Column.DataType == typeof(bool))
                {
                    #region Try to reuse a CheckBoxUIElement

                    //Since the grid sometimes re-uses UIElements, we need to check to make sure 
                    //the header does not already have a CheckBoxUIElement attached to it.
                    //If it does, we just get a reference to the existing CheckBoxUIElement,
                    //and reset its properties.
                    CheckBoxUIElement checkBoxUIElement =
                        parent.GetDescendant(typeof(CheckBoxUIElement))
                        as CheckBoxUIElement;

                    if (checkBoxUIElement == null)
                        checkBoxUIElement = new CheckBoxUIElement(parent);

                    // Use XP themes, if available.
                    if (checkBoxUIElement.Appearance == null)
                        checkBoxUIElement.Appearance = new Infragistics.Win.Appearance();
                    checkBoxUIElement.Appearance.ThemedElementAlpha = Alpha.Opaque;

                    #endregion Try to reuse a CheckBoxUIElement

                    #region Get a reference to the ColumnHeader

                    // Get the ColumnHeader that this CheckBoxUIElement is on.
                    Infragistics.Win.UltraWinGrid.ColumnHeader columnHeader =
                        checkBoxUIElement.GetAncestor(typeof(HeaderUIElement))
                        .GetContext(typeof(Infragistics.Win.UltraWinGrid.ColumnHeader))
                        as Infragistics.Win.UltraWinGrid.ColumnHeader;

                    // Just to be safe...
                    if (columnHeader == null)
                        return;

                    #endregion Get a reference to the ColumnHeader

                    #region Get the CheckState for the CheckBoxUIElement

                    // Check if we have already stored the value of this column's associated CheckState.
                    // If we have not, then add this column to the Hashtable and give it an 'Indeterminate' value.
                    // Otherwise, just get the column's CheckState from the HashTable.
                    // We need to store the CheckState values because the CheckBoxUIElements are destroyed and
                    // recreated often, so they do not maintain the CheckState value automatically.
                    string columnKey = columnHeader.Column.Key;
                    CheckState currentState = CheckState.Indeterminate;
                    if (!this.hashCheckStates.Contains(columnKey))
                        this.hashCheckStates.Add(columnKey, CheckState.Indeterminate);
                    else
                        currentState = (CheckState)this.hashCheckStates[columnKey];
                    checkBoxUIElement.CheckState = currentState;

                    #endregion Get the CheckState for the CheckBoxUIElement

                    #region Attach a handler for the ElementClick event

                    // Hook into the ElementClick of the CheckBoxUIElement
                    checkBoxUIElement.ElementClick += new UIElementEventHandler(this.OnCheckBoxUIElementElementClick);

                    #endregion Attach a handler for the ElementClick event

                    #region Add the CheckBoxUIElement to the Header's child elements collection

                    // Add the CheckBoxUIElement to the HeaderUIElement's collection of child elements.
                    parent.ChildElements.Add(checkBoxUIElement);

                    #endregion Add the CheckBoxUIElement to the Header's child elements collection

                    #region Adjust the location and size of the CheckBoxUIElement and it's neighboring element

                    //Position the CheckBoxUIElement. The number 3 here is used for 3
                    //pixels of padding between the CheckBox and the edge of the Header.
                    //The CheckBox is shifted down slightly so it is centered in the header.
                    checkBoxUIElement.Rect = new Rectangle(
                        parent.Rect.X + 3,
                        parent.Rect.Y + ((parent.Rect.Height - checkBoxUIElement.CheckSize.Height) / 2),
                        checkBoxUIElement.CheckSize.Width,
                        checkBoxUIElement.CheckSize.Height
                        );

                    //Get the TextUIElement - this is where the text for the 
                    //Header is displayed. We need this so we can push it to the right
                    //in order to make room for the CheckBox
                    TextUIElement textUIElement =
                        parent.GetDescendant(typeof(TextUIElement))
                        as TextUIElement;

                    //  Just to be safe...
                    if (textUIElement == null)
                        return;

                    //Push the TextUIElement to the right a little to make 
                    //room for the CheckBox. 3 pixels of padding are used again. 
                    textUIElement.Rect = new Rectangle(
                        checkBoxUIElement.Rect.Right + 3,
                        textUIElement.Rect.Y,
                        parent.Rect.Width - (checkBoxUIElement.Rect.Right - parent.Rect.X),
                        textUIElement.Rect.Height);

                    #endregion Adjust the location and size of the CheckBoxUIElement and it's neighboring element
                }
                else
                {
                    #region Dispose of any old CheckBoxUIElements

                    // If the column is not a boolean column, we do not want to have a checkbox in it
                    // Since UIElements can be reused by the grid, there is a chance that one of the
                    // HeaderUIElements that we added a checkbox to for a boolean column header
                    // will be reused in a column that is not boolean.  In this case, we must remove
                    // the checkbox so that it will not appear in an inappropriate column header.
                    CheckBoxUIElement checkBoxUIElement = parent.GetDescendant(typeof(CheckBoxUIElement)) as CheckBoxUIElement;
                    if (checkBoxUIElement != null)
                    {
                        parent.ChildElements.Remove(checkBoxUIElement);
                        checkBoxUIElement.Dispose();
                    }

                    #endregion Dispose of any old CheckBoxUIElements
                }
            }

        #endregion AfterCreateChildElements
        }

        #endregion IUIElementCreationFilter Implementation
    }

    #endregion HeaderCheckBoxCreationFilter
}