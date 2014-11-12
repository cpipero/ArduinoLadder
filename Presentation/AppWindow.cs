using Gtk;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;

namespace LadderLogic.Presentation
{
	using Controller;
	using Drawing;
	using Reader;
	using Surface;
	using Controller.State;
	using CTool;
	using CTool.FunctionRule;
	using StateType = Controller.State.StateType;

	public class AppWindow: Window
	{
		public AppWindow () : base ("Drawings view")
		{
			InitializeComponents ();
		}


		[Glade.Widget]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		Table table1;
		// ReSharper restore InconsistentNaming


		[Glade.Widget]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		ScrolledWindow scrolledwindow1;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		MenuItem  mNew;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		MenuItem  mOpen;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		MenuItem  mSave;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		MenuItem  mSaveAs;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		MenuItem  mExportToPng;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		MenuItem  mQuit;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		MenuItem  mEnvironment;
		// ReSharper restore InconsistentNaming

		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		MenuItem  mAbout;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		ComboBox  cbDefaultVar;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		ComboBox  cbExistVar;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		Entry  etNewVar;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		RadioButton  rbDefault;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		RadioButton  rbExisting;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		RadioButton  rbNew;
		// ReSharper restore InconsistentNaming


		[Glade.Widget]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		Table tblArdulino;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		ComboBox  cbDefaultVar1;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		ComboBox  cbExistVar1;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		Entry  etNewVar1;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		RadioButton  rbDefault1;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		RadioButton  rbExisting1;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		RadioButton  rbNew1;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		ComboBox  cbDefaultVar2;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		ComboBox  cbExistVar2;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		Entry  etNewVar2;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		RadioButton  rbDefault2;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		RadioButton  rbExisting2;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		RadioButton  rbNew2;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		ComboBox  cbDefaultVar3;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		ComboBox  cbExistVar3;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		Entry  etNewVar3;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		RadioButton  rbDefault3;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		RadioButton  rbExisting3;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		RadioButton  rbNew3;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		ComboBox  cbFunction;
		// ReSharper restore InconsistentNaming


		[Glade.WidgetAttribute]
		// ReSharper disable once UnassignedField.Compiler
		// ReSharper disable InconsistentNaming
		CheckButton chbOverride;
		// ReSharper restore InconsistentNaming


		[Glade.Widget()]
		Gtk.VBox vboxPalette;


		ElementDrawing _grid;


		void InitializeComponents()
		{
			var stream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream(AppController.Instance.Config.AppWindow);

			var glade = new Glade.XML(stream, AppController.Instance.Config.AppWindowName, null);
			if (stream != null)
			{
				stream.Close();
			}
			glade.Autoconnect(this);

			mNew.Activated += OnNew;
			mOpen.Activated += OnOpen;
			mSave.Activated += OnSave;
			mSaveAs.Activated += OnSaveAs;
			mQuit.Activated += OnQuit;
			mEnvironment.Activated += OnEnvironment;
			mAbout.Activated += OnAbout;
			mExportToPng.Activated += OnExportToPng;

			rbDefault.Clicked += OnVariableRadiobuttonCliecked;
			rbExisting.Clicked += OnVariableRadiobuttonCliecked;
			rbNew.Clicked += OnVariableRadiobuttonCliecked;

			rbDefault1.Clicked += OnVariableRadiobuttonCliecked1;
			rbExisting1.Clicked += OnVariableRadiobuttonCliecked1;
			rbNew1.Clicked += OnVariableRadiobuttonCliecked1;

			rbDefault2.Clicked += OnVariableRadiobuttonCliecked2;
			rbExisting2.Clicked += OnVariableRadiobuttonCliecked2;
			rbNew2.Clicked += OnVariableRadiobuttonCliecked2;

			rbDefault3.Clicked += OnVariableRadiobuttonCliecked3;
			rbExisting3.Clicked += OnVariableRadiobuttonCliecked3;
			rbNew3.Clicked += OnVariableRadiobuttonCliecked3;

			DisableProperties ();

			cbDefaultVar.Changed += OnVariableComboboxChanged;
			cbExistVar.Changed += OnVariableComboboxChanged;
			etNewVar.Changed += OnVariableComboboxChanged;

			cbDefaultVar1.Changed += OnVariableComboboxChanged;
			cbExistVar1.Changed += OnVariableComboboxChanged;
			etNewVar1.Changed += OnVariableComboboxChanged;

			cbDefaultVar2.Changed += OnVariableComboboxChanged;
			cbExistVar2.Changed += OnVariableComboboxChanged;
			etNewVar2.Changed += OnVariableComboboxChanged;

			cbDefaultVar3.Changed += OnVariableComboboxChanged;
			cbExistVar3.Changed += OnVariableComboboxChanged;
			etNewVar3.Changed += OnVariableComboboxChanged;

			cbFunction.Changed += OnFunctionChanged;
			chbOverride.Toggled += OnOverrideToggled;

			IconList = new[]{ new Gdk.Pixbuf (Assembly.GetExecutingAssembly (), AppController.Instance.Config.Icon) };
			Icon = IconList[0];

			var surfaces = AppController.Instance.GetPalette ();

			uint count = (uint)surfaces.Sum (s => s.Segments.Count);
			table1 = new Gtk.Table (2, count / 2, true);

			int index = 0;
			for (uint i = 0; i < count / 2; i++) {
				for (uint j = 0; j < 2; j++) {
					if (surfaces.Count () > index) {
						var surf = surfaces [index++];
						var el = new ElementDrawing(surf, 2, 2 );
						table1.Attach (el, i, i+1, j, j+(uint)surf.Segments.Count());
						el.Show ();
					}
				}
			}

			vboxPalette.Add (table1);

			table1.SetSizeRequest (300, 100);
			table1.Show ();

			_grid = new ElementDrawing(AppController.Instance.Surface, 24, 24);

			scrolledwindow1.AddWithViewport (_grid);

			_grid.Show ();

			var arduino = AppController.Instance.GetArduino ();

			var code = new ElementDrawing(arduino[0], 2, 2 );
			tblArdulino.Attach (code, 0, 1, 0, 1);
			code.CreateCode += CController.Instance.CreateCode;
			code.Show ();

		}


		void OnOverrideToggled (object sender, EventArgs e)
		{
			var c = AppController.Instance.CurrentState as PropertiesState;
			if (c != null && c.Element != null) {
				c.Element.OverrideFunction = chbOverride.Active;
			}
		}


		void OnFunctionChanged (object sender, EventArgs e)
		{
			var c = AppController.Instance.CurrentState as PropertiesState;
			if (c != null && c.Element != null) {
				if (c.Element.OverrideFunction) {
					FunctionType res;
					if (Enum.TryParse (cbFunction.ActiveText, out res)) {
						c.Element.Function = res;

						var state = AppController.Instance.CurrentState as PropertiesState;
						if (state != null && state.Element != null) {
							EnableProperties (state.Element);
						}
					}
				}
			}
		}


		void OnVariableComboboxChanged (object sender, EventArgs e)
		{
			SetElementVariables ();
		}


		void OnVariableEntryChanged (object sender, EventArgs e)
		{
			if (!AppController.Instance.GetPlcInputs ().Select (s => s.ToLowerInvariant ()).Contains (etNewVar.Text.ToLowerInvariant ()) &&
				!AppController.Instance.GetPlcOutputs ().Select (s => s.ToLowerInvariant ()).Contains (etNewVar.Text.ToLowerInvariant ())) {
				SetElementVariables ();
			}
		}


		public void OpenSourceDialog(string text)
		{
			var sourceDialog = new SourceDialog (this, text);
			sourceDialog.ShowDialog ();
		}


		void OnVariableRadiobuttonCliecked (object sender, EventArgs e)
		{
			if (rbDefault.Active) {
				cbDefaultVar.Sensitive = true;
				cbExistVar.Sensitive = false;
				etNewVar.Sensitive = false;
			}

			if (rbExisting.Active) {
				cbExistVar.Sensitive = true;
				cbDefaultVar.Sensitive = false;
				etNewVar.Sensitive = false;
			}

			if (rbNew.Active) {
				etNewVar.Sensitive = true;
				cbExistVar.Sensitive = false;
				cbDefaultVar.Sensitive = false;
			}
		}


		void OnVariableRadiobuttonCliecked1 (object sender, EventArgs e)
		{
			if (rbDefault1.Active) {
				cbDefaultVar1.Sensitive = true;
				cbExistVar1.Sensitive = false;
				etNewVar1.Sensitive = false;
			}

			if (rbExisting1.Active) {
				cbExistVar1.Sensitive = true;
				cbDefaultVar1.Sensitive = false;
				etNewVar1.Sensitive = false;
			}

			if (rbNew1.Active) {
				etNewVar1.Sensitive = true;
				cbExistVar1.Sensitive = false;
				cbDefaultVar1.Sensitive = false;
			}
		}


		void OnVariableRadiobuttonCliecked2 (object sender, EventArgs e)
		{
			if (rbDefault2.Active) {
				cbDefaultVar2.Sensitive = true;
				cbExistVar2.Sensitive = false;
				etNewVar2.Sensitive = false;
			}

			if (rbExisting2.Active) {
				cbExistVar2.Sensitive = true;
				cbDefaultVar2.Sensitive = false;
				etNewVar2.Sensitive = false;
			}

			if (rbNew2.Active) {
				etNewVar2.Sensitive = true;
				cbExistVar2.Sensitive = false;
				cbDefaultVar2.Sensitive = false;
			}
		}


		void OnVariableRadiobuttonCliecked3 (object sender, EventArgs e)
		{
			if (rbDefault3.Active) {
				cbDefaultVar3.Sensitive = true;
				cbExistVar3.Sensitive = false;
				etNewVar3.Sensitive = false;
			}

			if (rbExisting3.Active) {
				cbExistVar3.Sensitive = true;
				cbDefaultVar3.Sensitive = false;
				etNewVar3.Sensitive = false;
			}

			if (rbNew3.Active) {
				etNewVar3.Sensitive = true;
				cbExistVar3.Sensitive = false;
				cbDefaultVar3.Sensitive = false;
			}
		}


		public void BindDefaultInputs()
		{
			BindCombobox(cbDefaultVar, AppController.Instance.Config.PlcInputs.Split(';'));
			BindCombobox(cbDefaultVar1, AppController.Instance.Config.PlcInputs.Split(';'));
			BindCombobox(cbDefaultVar2, AppController.Instance.Config.PlcInputs.Split(';'));
			BindCombobox(cbDefaultVar3, AppController.Instance.Config.PlcInputs.Split(';'));
		}


		public void BindDefaultOutputs()
		{
			BindCombobox(cbDefaultVar, AppController.Instance.Config.PlcOutputs.Split(';'));
			BindCombobox(cbDefaultVar1, AppController.Instance.Config.PlcOutputs.Split(';'));
			BindCombobox(cbDefaultVar2, AppController.Instance.Config.PlcOutputs.Split(';'));
			BindCombobox(cbDefaultVar3, AppController.Instance.Config.PlcOutputs.Split(';'));
		}


		public void BindExistingVariables(Segment seg)
		{
			BindCombobox(cbExistVar, AppController.Instance.GetUserVariables());
			BindCombobox(cbExistVar1, AppController.Instance.GetUserVariables());
			BindCombobox(cbExistVar2, AppController.Instance.GetUserVariables());
			BindCombobox(cbExistVar3, AppController.Instance.GetUserVariables());
		}


		public void BindCombobox(ComboBox cb, string [] values)
		{
			cb.Clear ();

			var store = new ListStore (typeof(string));
			cb.Model = store;

			foreach (var input in values) {
				store.AppendValues (input);
			}

			var text = new CellRendererText {Style = Pango.Style.Oblique};
			//actorText.BackgroundGdk = new Gdk.Color(0x63,0,0);
			cb.PackStart(text,true);
			cb.AddAttribute (text, "text", 0);
		}


		public void DisableProperties()
		{
			cbDefaultVar.Sensitive = false;
			cbExistVar.Sensitive = false;
			etNewVar.Sensitive = false;
			rbDefault.Sensitive = false;
			rbExisting.Sensitive = false;
			rbNew.Sensitive = false;

			cbDefaultVar1.Sensitive = false;
			cbExistVar1.Sensitive = false;
			etNewVar1.Sensitive = false;
			rbDefault1.Sensitive = false;
			rbExisting1.Sensitive = false;
			rbNew1.Sensitive = false;


			cbDefaultVar2.Sensitive = false;
			cbExistVar2.Sensitive = false;
			etNewVar2.Sensitive = false;
			rbDefault2.Sensitive = false;
			rbExisting2.Sensitive = false;
			rbNew2.Sensitive = false;


			cbDefaultVar3.Sensitive = false;
			cbExistVar3.Sensitive = false;
			etNewVar3.Sensitive = false;
			rbDefault3.Sensitive = false;
			rbExisting3.Sensitive = false;
			rbNew3.Sensitive = false;

			cbFunction.Sensitive = false;
			chbOverride.Sensitive = false;
		}


		public void EnableProperties(Segment seg)
		{
			var c = seg.GetVariablesCount ();
			if (c > 0) {
				rbDefault.Sensitive = true;
				rbExisting.Sensitive = true;
				rbNew.Sensitive = true;
				OnVariableRadiobuttonCliecked (this, EventArgs.Empty);
			} else {
				DiasbaleVariable(cbDefaultVar, 
					cbExistVar, 
					etNewVar,
					rbDefault, 
					rbExisting, 
					rbNew);
			}

			if (c > 1) {
				rbDefault1.Sensitive = true;
				rbExisting1.Sensitive = true;
				rbNew1.Sensitive = true;
				OnVariableRadiobuttonCliecked1 (this, EventArgs.Empty);
			} else {
				DiasbaleVariable(cbDefaultVar1, 
					cbExistVar1, 
					etNewVar1,
					rbDefault1, 
					rbExisting1, 
					rbNew1);
			}

			if (c > 2) {
				rbDefault2.Sensitive = true;
				rbExisting2.Sensitive = true;
				rbNew2.Sensitive = true;			
				OnVariableRadiobuttonCliecked2 (this, EventArgs.Empty);
			} else {
				DiasbaleVariable(cbDefaultVar2, 
					cbExistVar2, 
					etNewVar2,
					rbDefault2, 
					rbExisting2, 
					rbNew2);
			}


			if (c > 3) {
				rbDefault3.Sensitive = true;
				rbExisting3.Sensitive = true;
				rbNew3.Sensitive = true;	
				OnVariableRadiobuttonCliecked3 (this, EventArgs.Empty);
			} else {
				DiasbaleVariable(cbDefaultVar3, 
					cbExistVar3, 
					etNewVar3,
					rbDefault3, 
					rbExisting3, 
					rbNew3);
			}

			cbFunction.Sensitive = true;
			chbOverride.Sensitive = true;
		}

		public void DiasbaleVariable(ComboBox d, 
			ComboBox u, 
			Entry n,
			RadioButton rd, 
			RadioButton ru, 
			RadioButton rn)
		{
			rd.Active = true;
			rd.Sensitive = false;
			ru.Sensitive = false;
			rn.Sensitive = false;	
			BindComboboxValue (d, string.Empty);
			BindComboboxValue (u, string.Empty);
			n.Text = string.Empty;
			d.Sensitive = false;
			u.Sensitive = false;
			n.Sensitive = false;
		}


		public void SetElementVariables()
		{
			var state = AppController.Instance.CurrentState as PropertiesState;
			if (state == null || state.Element == null) {
				return;
			}

			var vars = new List<Variable> ();

			string variable;
			VariableType t;

			var c = state.Element.GetVariablesCount ();

			if (c > 0) {
				GetPropertyData (cbDefaultVar, 
					cbExistVar, 
					etNewVar,
					rbDefault, 
					rbExisting, 
					rbNew, 
					out variable, 
					out t);

				if (variable != null) {
					vars.Add (new Variable{ Value = variable, Type = t });
				}
			}

			if (c > 1) {
				GetPropertyData (cbDefaultVar1, 
					cbExistVar1, 
					etNewVar1,
					rbDefault1, 
					rbExisting1, 
					rbNew1, 
					out variable, 
					out t);

				if (variable != null) {
					vars.Add (new Variable{ Value = variable, Type = t });
				}
			}

			if (c > 2) {
				GetPropertyData (cbDefaultVar2, 
					cbExistVar2, 
					etNewVar2,
					rbDefault2, 
					rbExisting2, 
					rbNew2, 
					out variable, 
					out t);

				if (variable != null) {
					vars.Add (new Variable{ Value = variable, Type = t });
				}
			}

			if (c > 3) {
				GetPropertyData (cbDefaultVar3, 
					cbExistVar3, 
					etNewVar3,
					rbDefault3, 
					rbExisting3, 
					rbNew3, 
					out variable, 
					out t);

				if (variable != null) {
					vars.Add (new Variable{ Value = variable, Type = t });
				}
			}

			if (vars.Any()) {
				AppController.Instance.SetElementVariable (vars);
			}
		}


		public void GetPropertyData(ComboBox d, 
			ComboBox u, 
			Entry n,
			RadioButton rd, 
			RadioButton ru, 
			RadioButton rn, 
			out string variable, 
			out VariableType t)
		{
			variable = string.Empty;
			t = VariableType.Default;
			if (rd.Active) {
				variable = d.ActiveText;
				t = VariableType.Default;
			}

			if (ru.Active) {
				variable = u.ActiveText;
				t = VariableType.User;
			}

			if (rn.Active) {
				variable = n.Text;
				t = VariableType.New;	
			}
		}


		public void BindElementVariables(VariableType t, string value, int index)
		{
			switch (index) {
			case 0:
				BindVariables (cbDefaultVar, 
					cbExistVar, 
					etNewVar, 
					rbDefault, 
					rbExisting, 
					rbNew,  
					t, 
					value);
				break;
			case 1:
				BindVariables (cbDefaultVar1, 
					cbExistVar1, 
					etNewVar1, 
					rbDefault1, 
					rbExisting1, 
					rbNew1,  
					t, 
					value);
				break;
			case 2:
				BindVariables (cbDefaultVar2, 
					cbExistVar2, 
					etNewVar2, 
					rbDefault2, 
					rbExisting2, 
					rbNew2,  
					t, 
					value);
				break;
			case 3:
				BindVariables (cbDefaultVar3, 
					cbExistVar3, 
					etNewVar3, 
					rbDefault3, 
					rbExisting3, 
					rbNew3,  
					t, 
					value);
				break;
			}
		}

		private void BindComboboxValue(ComboBox d, 
			string value)
		{
			TreeIter iter;
			d.Model.GetIterFirst (out iter);
			do {
				var thisRow = new GLib.Value ();
				d.Model.GetValue (iter, 0, ref thisRow);
				var cur = thisRow.Val as string;
				if (cur != null && cur.Equals (value)) {
					d.SetActiveIter (iter);
					break;
				}
			} while(d.Model.IterNext (ref iter));
		}


		private void BindVariables(ComboBox d, 
			ComboBox u, 
			Entry n, 
			RadioButton rd, 
			RadioButton ru, 
			RadioButton rn,  
			VariableType t, 
			string value)
		{
			switch (t) {
			case VariableType.Default:
				BindComboboxValue (d, value);
				rd.Active = true;
				n.Text = string.Empty;
				break;
			case VariableType.New:
				n.Text = value;
				rn.Active = true;
				break;
			case VariableType.User:
				BindComboboxValue (u, value);
				ru.Active = true;
				n.Text = string.Empty;
				break;
			}
		}

		public void BindFunction(Segment s)
		{
			var seg = s;
			var c = AppController.Instance.CurrentState as PropertiesState;
			if (c != null && c.Element != null) {
				seg = c.Element;
			}

			var functions = seg.Type.GetFunctionsForElement ();

			BindCombobox (cbFunction, 
				functions.Select (f => Enum.GetName (typeof(FunctionType), f))
				.ToArray ());

			string comment;
			var func = seg.OverrideFunction ? seg.Function : seg.Type.GetRuleForElement().Resolve(seg, out comment);

			BindComboboxValue (cbFunction, Enum.GetName(typeof(FunctionType), func));
			chbOverride.Active = seg.OverrideFunction;
		}

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
		void HandleKeyReleaseEvent (object o, KeyReleaseEventArgs args)
// ReSharper restore UnusedParameter.Local
// ReSharper restore UnusedMember.Local
		{
			if (args.Event.Key == Gdk.Key.Delete && AppController.Instance.CurrentState.Type == StateType.ElementState) {
				AppController.Instance.DeleteCurrentElement ();
			}
		}


		void HandleSizeAllocateEvent(object o, SizeAllocatedArgs e)
		{
			var width = e.Allocation.Width - 330;
			_grid.SetSizeRequest (
				width, 
				(int)(((double)width / AppController.Instance.Config.ContactsCount) *
					AppController.Instance.Config.RowsCount));
		}


		void OnNew (object sender, EventArgs e)
		{
			AppController.Instance.FileName = string.Empty;
			AppController.Instance.ResetSurface ();
			AppController.Instance.ClearSelection ();
			_grid.Surface.QueueDraw ();
		}


		void OnOpen (object sender, EventArgs e)
		{
			var fc=
				new FileChooserDialog("Choose the file to open",
					this,
					FileChooserAction.Open,
					"Cancel",ResponseType.Cancel,
					"Open",ResponseType.Accept);

			fc.SetPosition (WindowPosition.Center);
			if (fc.Run() == (int)ResponseType.Accept) 
			{
				if (!AppController.Instance.Surface.IsEmpty ()) {
					string fileName = Process.GetCurrentProcess ().MainModule.FileName.Replace (".vshost", "");
					ProcessStartInfo info = new ProcessStartInfo (fileName);
					info.UseShellExecute = false;
					info.Arguments = fc.Filename;
					Process processChild = Process.Start (info);
				} else {
					AppController.Instance.FileName = fc.Filename;
					var saved = ConfigManager.Read<PrimitivesSurface> (AppController.Instance.FileName);
					AppController.Instance.ResetSurface ();
					AppController.Instance.ReloadSurface (saved);
					_grid.Surface = AppController.Instance.Surface;
					_grid.Surface.QueueDraw ();
				}
			}
			//Don't forget to call Destroy() or the FileChooserDialog window won't get closed.
			fc.Destroy();
		}


		void OnSave (object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty (AppController.Instance.FileName)) {
				OnSaveAs (sender, e);
			} else {
				ConfigManager.Write<PrimitivesSurface> (AppController.Instance.Surface, AppController.Instance.FileName);
			}
		}		


		void OnSaveAs (object sender, EventArgs e)
		{
			var fc=
				new FileChooserDialog("Enter the file to save",
					this,
					FileChooserAction.Save,
					"Cancel",ResponseType.Cancel,
					"Save",ResponseType.Accept);

			fc.SetPosition (WindowPosition.Center);
			if (fc.Run() == (int)ResponseType.Accept) 
			{
				AppController.Instance.FileName = fc.Filename;
				ConfigManager.Write<PrimitivesSurface> (AppController.Instance.Surface, AppController.Instance.FileName);
			}
			//Don't forget to call Destroy() or the FileChooserDialog window won't get closed.
			fc.Destroy();
		}



		void OnExportToPng (object sender, EventArgs e)
		{
			var fc=
				new FileChooserDialog("Enter the file to save",
					this,
					FileChooserAction.Save,
					"Cancel",ResponseType.Cancel,
					"Save",ResponseType.Accept);

			fc.SetPosition (WindowPosition.Center);
			if (fc.Run() == (int)ResponseType.Accept) 
			{
				_grid.ExportToPng (fc.Filename);
				Process.Start (fc.Filename);
			}
			//Don't forget to call Destroy() or the FileChooserDialog window won't get closed.
			fc.Destroy();
		}


		static void OnQuit (object sender, EventArgs e)
		{
			Application.Quit ();
		}


		void OnEnvironment (object sender, EventArgs e)
		{
			new EnvironmentVariablesDialog (this).ShowDialog ();
		}


		void OnAbout (object sender, EventArgs e)
		{			
			new AboutDialog (this).ShowDialog();
		}


// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
		void OnWindowDeleteEvent (object sender, DeleteEventArgs a) 
// ReSharper restore UnusedParameter.Local
// ReSharper restore UnusedMember.Local
		{
			a.RetVal = true;
			Application.Quit ();
		}
	}
}

