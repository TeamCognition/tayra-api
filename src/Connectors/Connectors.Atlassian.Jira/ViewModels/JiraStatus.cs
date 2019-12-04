using System;
using Newtonsoft.Json;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class JiraStatus : IEquatable<JiraStatus> //For Distinct functionality
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("statusCategory")]
        public JiraStatusCategory Category { get; set; }

        public bool Equals(JiraStatus other)
        {

            //Check whether the compared object is null. 
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data. 
            if (Object.ReferenceEquals(this, other)) return true;

            //Are properties equal. 
            return Id.Equals(other.Id) && Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {

            //Get hash code for the Name field if it is not null. 
            int hashObjName = Name == null ? 0 : Name.GetHashCode();

            //Get hash code for the Code field. 
            int hashObjCode = Id.GetHashCode();

            //Calculate the hash code for the status. 
            return hashObjName ^ hashObjCode;
        }
    }
}
