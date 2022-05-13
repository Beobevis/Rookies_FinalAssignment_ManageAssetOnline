import React, { useState } from "react";
import axios from "axios";
export function CategoryAPI() {
  const [cateData, setCateData] = useState([]);
  return axios
    .get(`${process.env.REACT_APP_UNSPLASH_CATEGORY}`)
    .then((response) => {
      setCateData(response);

    });
}
