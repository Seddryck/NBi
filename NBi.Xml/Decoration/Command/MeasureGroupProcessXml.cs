using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NBi.Xml.Decoration.Command
{
    public class MeasureGroupProcessXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("partitions")]
        public string PartitionsDefinition { get; set; }

        [XmlIgnore()]
        public IEnumerable<int> Partitions
        {
            get
            {
                var partitions = new List<int>();

                if (string.IsNullOrEmpty(PartitionsDefinition) 
                        || string.IsNullOrEmpty(PartitionsDefinition.Replace(" ", "")) 
                        || PartitionsDefinition.Replace(" ", "") == "*")
                    return partitions;

                var partitionGroups = PartitionsDefinition.Replace(" ","").Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var partitionGroup in partitionGroups)
	            {
		            if (partitionGroup.Contains("-"))
                    {
                        var intervals = partitionGroup.Split('-');
                        if (intervals.Count()!=2)
                            throw new ArgumentException(string.Format("The range of partitions '{0}' must contain a start and an end.", partitionGroup));

                        int start, end;
                        if (!Int32.TryParse(intervals[0], out start))
                            throw new ArgumentException(string.Format("The first part of the range of partitions '{0}' must be an integer.", partitionGroup));
                        if (!Int32.TryParse(intervals[1], out end))
                            throw new ArgumentException(string.Format("The second part of the range of partitions '{0}' must be an integer.", partitionGroup));
                        for (int i = start; i <= end; i++)
			                partitions.Add(i);
                    }
                    else
                        partitions.Add(Int32.Parse(partitionGroup));
	            }
                return partitions.Distinct();
            }
        }

        public bool IsAllPartitions
        {
            get
            {
                return (Partitions.Count() == 0);
            }
        }

        public MeasureGroupProcessXml()
        {
        }
    }
}
