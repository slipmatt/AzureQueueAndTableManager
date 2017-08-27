using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureManager.TableEntities
{
    public class MyEntity: TableEntity
    {
        public MyEntity(Guid id, int trackId)
        {
            this.PartitionKey = id.ToString();
            this.RowKey =  trackId.ToString();
        }

        public MyEntity() { }

        public int TrackId { get; set; }

        public string DateInserted { get; set; }

    }
}
