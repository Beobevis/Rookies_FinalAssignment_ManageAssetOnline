import { Button, Table } from "antd";
import axios from "axios";
import fileDownload from "js-file-download";
import React, { useEffect, useState } from "react"


export default function ReportPage(){
  const [data, setData] = useState([]);
//   const [exportData, setExportData] = useState([]);
  useEffect(() =>{
    axios
    .get(`${process.env.REACT_APP_UNSPLASH_REPORT}`,{})
    .then((response) =>{
      setData(response.data);
    })
    .catch(() =>{

    })
  },[])

  const columns = [

    {
        title: "Category",
        dataIndex: "categoryName",
        sortDirections: ['descend', 'ascend'],
        key: "categoryName",
        
        sorter: (a, b) => 
        {
            if (a.categoryName > b.categoryName) {
                return -1;
            }
            if (b.categoryName > a.categoryName) {
                return 1;
            }
            return 0;
        },
    },
    {
        title: "Total",
        dataIndex: "total",
        key: "total",
        sorter: (a, b) => {
            if (a.total > b.total) {
                return -1;
            }
            if (b.total > a.total) {
                return 1;
            }
            return 0;
        },
    },
    {
        title: "Assigned",
        dataIndex: "assigned",
        key: "assigned",
        sorter: (a, b) => {
          if (a.assigned > b.assigned) {
              return -1;
          }
          if (b.assigned > a.assigned) {
              return 1;
          }
          return 0;
      },
    },
    {
        title: "Available",
        dataIndex: "available",
        key: "available",
        sorter: (a, b) => {
            if (a.available > b.available) {
                return -1;
            }
            if (b.available > a.available) {
                return 1;
            }
            return 0;
        },
    },
    {
        title: "Not Available",
        dataIndex: "notavailable",
        key: "notavailable",
        sorter: (a, b) => {
            if (a.notavailable > b.notavailable) {
                return -1;
            }
            if (b.notavailable > a.notavailable) {
                return 1;
            }
            return 0;
        },
    },
    {
        title: "Waiting For Recycling",
        dataIndex: "waitingforrecycling",
        key: "waitingforrecycling",
        sorter: (a, b) => {
          if (a.waitingforrecycling > b.waitingforrecycling) {
              return -1;
          }
          if (b.waitingforrecycling > a.waitingforrecycling) {
              return 1;
          }
          return 0;
      },
    },
    {
      title: "Recycled",
      dataIndex: "recycled",
      key: "recycled",
      sorter: (a, b) => {
        if (a.recycled > b.recycled) {
            return -1;
        }
        if (b.recycled > a.recycled) {
            return 1;
        }
        return 0;
    },
  },
];
const handleExports = ()=>{

  axios.get(`${process.env.REACT_APP_UNSPLASH_REPORT_EXPORTFILE}`,
  {responseType:'blob'})
  .then((response )=>{
    fileDownload(response.data, 'report.xlsx')
  })
}
//   useEffect(()=>{
//     axios
//     .get(`${process.env.REACT_APP_UNSPLASH_REPORT_EXPORTFILE}`,{})
//     .then((response)=>{
//       setExportData(response.data);
//     })
//     .catch((error)=>{
//       console.error(error)
//     })
//   },[])

  return (
  <>
      <p style={{ display :'block' ,fontSize:"20px", margin : '0 auto', textAlign:'left', color : ' red', fontWeight : 'bold', paddingBottom:"20px"}}>Report Page</p>
      <Button   style={{ background: "#e30c18",margin: "auto", color: "white",float: "right"}}  onClick={handleExports}>Export</Button>
  <div >

  <Table
      size = "middle"
  columns={columns}
  dataSource={data.sort((a, b) => {
    if (a.categoryName.trim().toLowerCase() > b.categoryName.trim().toLowerCase()) {
        return 1;
    }
    if (b.categoryName.trim().toLowerCase() > a.categoryName.trim().toLowerCase()) {
        return -1;
    }
    return 0;
})}
  >

  </Table>  
  </div>
  
  </>);
}