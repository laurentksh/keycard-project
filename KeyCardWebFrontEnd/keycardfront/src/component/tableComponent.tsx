import axios from "axios";
import * as React from "react";
import { Component, useEffect } from "react";
import { useState } from "react";
import { PunchInterface } from "../interface/PunchInterface";


const TableComponent = (): JSX.Element => {
  const [punchList, setPunchList] = useState<PunchInterface[]>([]);

  useEffect(async () => {
    setPunchList(await getPunches());
  }, []);

  async function getPunches() : Promise<PunchInterface[]> {
    const response = await axios.get('/api/v1/Punch/history');
    console.log(response);
    return response.data;
  }


  return (
    <div className="table m-auto">
      <p>Résumé</p>
      <div className="flex flex-col m-auto">
        <div className="overflow-x-auto sm:-mx-6 lg:-mx-8">
          <div className="py-4 inline-block min-w-full sm:px-6 lg:px-8">
            <div className="overflow-hidden">
              <table className="min-w-full text-center">
                <thead className="border-b bg-gray-800">
                  <tr>

                    <th scope="col" className="text-sm font-medium text-white px-6 py-4">
                      ID
                    </th>
                    <th scope="col" className="text-sm font-medium text-white px-6 py-4">
                      Date
                    </th>
                    <th scope="col" className="text-sm font-medium text-white px-6 py-4">
                      Source
                    </th>
                    
                  </tr>
                </thead>  
                <tbody>

                  <tr className="bg-white border-b">

                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">1</td>

                    <td className="text-sm text-gray-900 font-light px-6 py-4 whitespace-nowrap">Mark</td>
                    
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default TableComponent;