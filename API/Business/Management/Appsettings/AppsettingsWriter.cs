using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Newtonsoft.Json;



namespace Business.Management.Appsettings
{
    public class AppsettingsWriter : IAppsettingsWriter
    {

        private IServiceResultFactory _resultFact;



        public AppsettingsWriter(IServiceResultFactory resultFact)
        {
            _resultFact = resultFact;
        }



        public IServiceResult<string> AddOrUpdateAppSetting<T>(string sectionPathKey, T value)
        {           
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            
                string json = File.ReadAllText(filePath);
            
                dynamic Appsettings = Newtonsoft.Json.JsonConvert.DeserializeObject(json) ?? new { };
            
                SetValue(sectionPathKey, Appsettings, value);
            
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(Appsettings, Newtonsoft.Json.Formatting.Indented);
            
                File.WriteAllText(filePath, output);
            
                return _resultFact.Result($"'{sectionPathKey}' : '{value}'", true);
            }
            catch (Exception ex)
            {
                return _resultFact.Result($"'{sectionPathKey}' : '{value}'", true, ex.HResult == -2146233088
                    ? $"Section '{sectionPathKey}' was not found in Appsettings!"
                    : $"Failed to write into section '{sectionPathKey}' in Appsettings! Reason: {ex.Message}");
            }
        }



        private void SetValue<T>(string sectionPathKey, dynamic jsonObj, T value)
        {
            var remainingSections = sectionPathKey.Split(":", 2);

            var currentSection = remainingSections[0];

            if (remainingSections.Length > 1)
            {
                // continue with the procress, moving down the tree:
                var nextSection = remainingSections[1];

                SetValue(nextSection, jsonObj[currentSection], value);
            }
            else
            {
                // we've got to the end of the tree, set the value:
                // throws EX if adding value to non existent NULL section in json
                var section = jsonObj[currentSection];
                var x = JsonConvert.SerializeObject(value);

                jsonObj[currentSection] = value;
            }
        }

    }
}
