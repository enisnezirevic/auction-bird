﻿import axios from "axios";
import {environment} from "../../environments/environment.ts";

export const recommendationsService = {
  async recommendationsRegularUser(count: number) {
    const response = await axios.get(`${environment.apiUrl}/recommendations/regular`, {params: {count}});
    return response.data;
  },

  async recommendationsSignedInUser(username: string, count: number) {
    const response = await axios.get(`${environment.apiUrl}/recommendations/registered`, {
      params: {
        username,
        count
      }
    });
    return response.data;
  }
};