using UnityEngine;

namespace PlayFab.Examples.Client
{
    /// <summary>
    /// This example will have poor performance for a real title with lots of items.
    /// However, it's a very consise example for a test-title, with a small number of CatalogItems.
    /// 
    /// This is example code for all the API's described here: PlayFab Inventory System - Basic Inventory Guide
    /// This file contains calls to each of the functions described, an old-style Unity-gui to demonstrate the inventory changes taking place, and the prerequisite login and setup code.
    /// </summary>
    public class C_InventoryExampleGui : PfExampleGui
    {
        void Awake()
        {
            InventoryExample.SetUp();
        }

        #region Unity GUI
        public override void OnExampleGUI(ref int rowIndex)
        {
            bool isLoggedIn = PlayFabClientAPI.IsClientLoggedIn();
            bool charsValid = isLoggedIn && PfSharedModelEx.globalClientUser.clientCharacterModels.Count > 0;
            int colIndex;

            Button(isLoggedIn, rowIndex, 0, "Refresh User Inv", InventoryExample.GetUserInventory);
            TextField(isLoggedIn, rowIndex, 1, ref PfSharedModelEx.globalClientUser.userInvDisplay);
            rowIndex++;
            // Purchase Items
            TextField(isLoggedIn, rowIndex, 0, "Purchase User Item:");
            if (PfSharedModelEx.clientCatalog != null)
            {
                colIndex = 1;
                foreach (var catalogPair in PfSharedModelEx.clientCatalog)
                    Button(isLoggedIn, rowIndex, colIndex++, catalogPair.Value.DisplayName, InventoryExample.PurchaseUserItem(catalogPair.Key));
            }
            rowIndex++;
            // Consume Appropriate User Items
            TextField(isLoggedIn, rowIndex, 0, "Consume:");
            if (PfSharedModelEx.globalClientUser.clientUserItems != null)
            {
                colIndex = 1;
                for (int i = 0; i < PfSharedModelEx.globalClientUser.clientUserItems.Count; i++)
                    if (PfSharedModelEx.consumableItemIds.Contains(PfSharedModelEx.globalClientUser.clientUserItems[i].ItemId))
                        Button(isLoggedIn, rowIndex, colIndex++, PfSharedModelEx.globalClientUser.clientUserItems[i].DisplayName, InventoryExample.ConsumeUserItem(PfSharedModelEx.globalClientUser.clientUserItems[i].ItemInstanceId));
            }
            rowIndex++;
            // Open Appropriate User Containers
            TextField(isLoggedIn, rowIndex, 0, "Open:");
            if (PfSharedModelEx.globalClientUser.clientUserItems != null)
            {
                colIndex = 1;
                for (int i = 0; i < PfSharedModelEx.globalClientUser.clientUserItems.Count; i++)
                    if (PfSharedModelEx.containerItemIds.Contains(PfSharedModelEx.globalClientUser.clientUserItems[i].ItemId))
                        Button(isLoggedIn, rowIndex, colIndex++, PfSharedModelEx.globalClientUser.clientUserItems[i].DisplayName, InventoryExample.UnlockUserContainer(PfSharedModelEx.globalClientUser.clientUserItems[i].ItemId));
            }
            rowIndex++;
            rowIndex++;

            foreach (var charPair in PfSharedModelEx.globalClientUser.clientCharacterModels)
            {
                CharacterModel tempCharacter;
                if (!PfSharedModelEx.globalClientUser.clientCharacterModels.TryGetValue(charPair.Value.characterId, out tempCharacter))
                    continue;
                PfInvClientChar eachCharacter = tempCharacter as PfInvClientChar;
                if (eachCharacter == null || eachCharacter.inventory == null)
                    continue;

                Button(charsValid, rowIndex, 0, "Refresh " + charPair.Value.characterName + " Inv", eachCharacter.GetInventory);
                TextField(charsValid, rowIndex, 1, eachCharacter.inventoryDisplay);
                rowIndex++;
                // Grant Char Items
                TextField(charsValid, rowIndex, 0, "Purchase " + charPair.Value.characterName + " Item:");
                if (PfSharedModelEx.clientCatalog != null)
                {
                    colIndex = 1;
                    foreach (var catalogPair in PfSharedModelEx.clientCatalog)
                        Button(charsValid, rowIndex, colIndex++, catalogPair.Value.DisplayName, eachCharacter.PurchaseCharacterItem(catalogPair.Key));
                }
                rowIndex++;
                // Consume Appropriate Character Items
                TextField(charsValid, rowIndex, 0, "Consume:");
                colIndex = 1;
                for (int i = 0; i < eachCharacter.inventory.Count; i++)
                    if (PfSharedModelEx.consumableItemIds.Contains(eachCharacter.inventory[i].ItemId))
                        Button(charsValid, rowIndex, colIndex++, eachCharacter.inventory[i].DisplayName, eachCharacter.ConsumeItem(eachCharacter.inventory[i].ItemInstanceId));

                rowIndex++;
                // Open Appropriate Character Containers
                TextField(charsValid, rowIndex, 0, "Open:");
                colIndex = 1;
                for (int i = 0; i < eachCharacter.inventory.Count; i++)
                    if (PfSharedModelEx.containerItemIds.Contains(eachCharacter.inventory[i].ItemId))
                        Button(charsValid, rowIndex, colIndex++, eachCharacter.inventory[i].DisplayName, eachCharacter.UnlockContainer(eachCharacter.inventory[i].ItemId));
                rowIndex++;
                rowIndex++;
            }
        }
        #endregion Unity GUI
    }
}
