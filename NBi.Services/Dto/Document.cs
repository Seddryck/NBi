namespace NBi.Service.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class Document : DocumentBase
    {
        private static int count = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        public Document()
        {
            this.Schema = new Schema();
            this.Style = new Style();
            this.Id = count++;
        }

        public int Id { get; private set; }

        /// <summary>
        /// Gets the schema.
        /// </summary>
        public Schema Schema { get; private set; }

        /// <summary>
        /// Gets the style.
        /// </summary>
        public Style Style { get; private set; }
    }
}