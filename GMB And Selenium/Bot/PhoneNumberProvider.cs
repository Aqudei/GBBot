using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plivo;
using Plivo.Resource.PhoneNumber;

namespace GMB_And_Selenium.Bot
{
    class PhoneNumberProvider
    {
        private readonly PlivoApi _api;
        private readonly ConcurrentQueue<PhoneNumber> _phoneNumbers
            = new ConcurrentQueue<PhoneNumber>();

        public PhoneNumberProvider()
        {
            var line = File.ReadAllLines("Key.txt");

            var id = line[0].Trim("\r\n\t ".ToCharArray());
            var token = line[1].Trim("\r\n\t ".ToCharArray());

            _api = new PlivoApi(id, token);
            FetchNumbers();
        }

        private void FetchNumbers()
        {
            var response = _api.PhoneNumber.List("US", type: "tollfree");
            var numbers = response.ToArray();
            if (!numbers.Any())
                return;

            foreach (var phoneNumber in numbers)
            {
                _phoneNumbers.Enqueue(phoneNumber);
            }
        }

        public Task<string> GetNumberAsync()
        {
            return Task.Run(() => GetNumber());
        }

        public string GetNumber()
        {
            _phoneNumbers.TryDequeue(out var phoneNumber);

            if (phoneNumber == null)
            {
                FetchNumbers();
            }

            _phoneNumbers.TryDequeue(out phoneNumber);
            if (phoneNumber == null)
                throw new Exception("Unable to get new number");

            return phoneNumber.Number;
        }
    }
}
