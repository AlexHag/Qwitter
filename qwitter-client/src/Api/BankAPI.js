import { useMemo } from "react";
import { backend } from "./backend";
import { useAuth } from "../Auth/AuthProvider";

export const authHeader = (token) => ({
  Authorization: `Bearer ${token}`
});

export class BankAPI {
  constructor(apiUrl, user, token) {
    this.apiUrl = apiUrl;
    this.user = user;
    this.token = token;
  }

  getAccounts = () => 
    backend.GET({
      url: `${this.apiUrl}/bank-account/user/${this.user.userId}`,
      headers: authHeader(this.token)
    });
  
  createBankAccount = (accountName, currency, accountType) => 
    backend.POST({
      url: `${this.apiUrl}/bank-account`,
      headers: authHeader(this.token),
      body: {
        accountName,
        currency,
        accountType,
        userId: this.user.userId
      }
    }); 
  
  internalBankTransfer = (fromAccountId, toAccountId, amount) =>
    backend.POST({
      url: `${this.apiUrl}/internal-bank-transfer`,
      headers: authHeader(this.token),
      body: {
        userId: this.user.userId,
        fromAccountId,
        toAccountId,
        amount
      }
    });
}

export const useBankAPI = () => {
  const auth = useAuth();

  return useMemo(
    () => new BankAPI("https://localhost:7005", auth.user, auth.token),
    [auth.user, auth.token]
  )
}
