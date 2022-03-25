import React from 'react';
import logo from './logo.svg';
import { Component } from "react";
import TableComponent from './component/tableComponent';
import './App.css';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <p>Application de trimbrage</p>
      </header>

      <body>
        <h1>Trimbrage du jour</h1>
        <TableComponent />
      </body>

      <footer>
        <p>test</p>
      </footer>
    </div>
  );
}

export default App;
