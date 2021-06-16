using System;
using DevinoTest.Services;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using DevinoTest.Model;
using System.Net.Http.Headers;
using System.Collections.Generic;
using DevinoTest.Model.Account;
using DevinoTest.Model.SmsMessages;
using System.Text;
using DevinoTest.Model.SmsMessages.Result;

namespace DevinoTest
{
    class Program
    {
        static string Path = "https://api.devino.online/";
        static string Key = "d398425c-e566-4be2-b45e-7fcda7436be5";
        static string base64 = "ZWRkaWU6TXQtMzc2Mzg1";
        static readonly HttpClient client = new HttpClient();       
                
        public static void headersApiAuth(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Key", Key);
        }
        public static async Task<bool> getFinanceStatus()
        {
            headersApiAuth(client);
            var content = await client.GetStringAsync($"{Path}billing-api/companies/current/account");
            AccountRoot deserializeAccount = JsonConvert.DeserializeObject<AccountRoot>(content);
            if (deserializeAccount.result.account.balance >= 100)
            {                
                return true;
            }
            else
            {
                Console.WriteLine($"Средств недостаточно для рассылки. На вашем счету- {deserializeAccount.result.account.balance} у.е.");
                Console.WriteLine($"Пополните счет!");
                return false;
            }

        }
        public static async Task SmsMessaging()
        {
            ListOutputMessage messageList = new ListOutputMessage();
            foreach (var message in SmsService.GetSmsList())
            {
                Message sms = new Message();
                sms.from = "DTSMS";
                sms.to = message.Phone;
                sms.text = message.Message;
                messageList.messages.Add(sms);
            }
            if (await getFinanceStatus()==true)
            {
                headersApiAuth(client);
                var stringContent = new StringContent(JsonConvert.SerializeObject(messageList, Formatting.Indented), Encoding.UTF8, "application/json");                
                HttpResponseMessage response = await client.PostAsync($"{Path}/sms/messages", stringContent);
                var content = JsonConvert.DeserializeObject<RootMessage>(await response.Content.ReadAsStringAsync());                
                foreach (var message in content.result)
                {
                if (message.code != "OK")
                {
                    Console.WriteLine($"Сообщение не отправлено - code {message.code},описание - {message.description}");
                    Console.WriteLine($"Описание - {message.description}");
                    Console.WriteLine();

                }
                else
                {
                    Console.WriteLine($"code {message.code}, messageId {message.messageId} ");
                    Console.WriteLine();
                }
                }
            }
            
        }
        public static async Task Main(string[] args)
        {
            await SmsMessaging();
            Console.ReadLine();
        }
    }
}
