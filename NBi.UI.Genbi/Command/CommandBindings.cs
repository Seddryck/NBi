using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NBi.UI.Genbi.Command
{
	class CommandBindings : List<ICommandBinding>
	{
		public void Add(ICommand command, ToolStripItem item)
		{
			this.Add(new ToolStripCommandBinding(command, item));
		}

		public void Add(ICommand command, Button item)
		{
			this.Add(new ButtonCommandBinding(command, item));
		}

		public void Remove(ICommand command, object item)
		{
			(from b in this
			where b.Command == command && b.Trigger == item
			select b)
				.ToList()
				.ForEach(b =>
					{
						this.Remove(b);
						b.Unbind();
					});
		}
	}
}