#region Copyright © 2001-2015 Eliwell Controls srl

// All rights are reserved.
// Reproduction or transmission in whole or in part, in any form
// or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename: GoogleSpreadsheetAmbassador.cs

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace HeroicFlux.Model
{
    public class GoogleSpreadsheetAmbassador
    {
        #region Instance Public Members
       
        public GoogleSpreadsheetAmbassador(String spreadSheetName)
        {
            GoogleSheetExample.Main();
            
            const string serviceAccountEmail = "78348221941-d0cua7il209ofk6fsquogv5rppu3bmj9@developer.gserviceaccount.com";

            var certificate = new X509Certificate2("Key.p12", "notasecret", X509KeyStorageFlags.Exportable);

            var credential =
                new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(serviceAccountEmail) // Create credential using certificate
                    {
// This scope is for spreadsheets, check Google scope FAQ for others
                        Scopes = new[]  //{ SheetsService.Scope.SpreadsheetsReadonly }
                        {"https://spreadsheets.google.com/feeds/" }
                    }.FromCertificate(certificate));

            credential.RequestAccessTokenAsync(CancellationToken.None).Wait(); //request token

            var requestFactory = new GDataRequestFactory("Some Name");
            requestFactory.CustomHeaders.Add(string.Format("Authorization: Bearer {0}", credential.Token.AccessToken));

            _spreadSheetName = spreadSheetName;

            _service = new SpreadsheetsService(_spreadSheetName) {RequestFactory = requestFactory};
            //_service.setUserCredentials("erik.burigo@gmail.com", "googleIpsogonofant3");

            var query = new SpreadsheetQuery();
            var spreadsheet = _service.Query(query);
            var entry = spreadsheet.Entries.OfType<SpreadsheetEntry>()
                .FirstOrDefault(e => e.Title.Text.ToLowerInvariant() == _spreadSheetName.ToLowerInvariant());
            _worksheetFeed = entry.Worksheets;
        }

        public IEnumerable<ListEntry> FromSpreadsheet(String workSheetName)
        {
            var propertyTokensSheet = _worksheetFeed.Entries.OfType<WorksheetEntry>()
                .FirstOrDefault(e => e.Title.Text.ToLowerInvariant() == workSheetName.ToLowerInvariant());

            var listFeedLink = propertyTokensSheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            var listQuery = new ListQuery(listFeedLink.HRef.ToString());
            var listFeed = _service.Query(listQuery);

            foreach (ListEntry row in listFeed.Entries)
            {
                yield return row;
            }

            Console.WriteLine(propertyTokensSheet.Summary);
        }

        public String Get(ListEntry row, String columnName)
        {
            return Get<String>(row, columnName);
        }

        public T Get<T>(ListEntry row, String columnName)
        {
            columnName = columnName.ToLowerInvariant().Replace(" ", "");
            var element = row.Elements.OfType<ListEntry.Custom>().FirstOrDefault(c => c.LocalName == columnName);

            var text = element.Value;

            if (typeof (T) == typeof (String))
                return (T) (text as Object);

            if (typeof (T) == typeof (Int32))
            {
                text.Trim();
                if (text == "")
                    text = "0";
                return (T) (Convert.ToInt32(text) as Object);
            }

            return default(T);
        }

        #endregion

        #region Instance Non-Public Members

        private readonly SpreadsheetsService _service;

        private readonly String _spreadSheetName;

        private readonly WorksheetFeed _worksheetFeed;

        #endregion
    }
}