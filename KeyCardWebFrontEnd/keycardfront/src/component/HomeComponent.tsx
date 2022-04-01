import * as React from "react";
import { Component } from "react";
import { useState } from "react";
import '../Home.css';
import TableComponent from "./tableComponent";

const Home = (): JSX.Element => {
  return (
      <div>
          <TableComponent />
      </div>
  );
}

export default Home;
