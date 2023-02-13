import React, { Component } from 'react';
import Dropdown from 'react-dropdown';
import 'react-dropdown/style.css';
import { FilterInfo } from '../components/FilterInfo';
import { TableInfo } from '../components/TableInfo';


export class LogPage extends Component {
    static displayName = LogPage.name;

    constructor(props) {
        super(props);
        this.state = {
            data: [],
            logTypes: [],
            dropDownLogTypes:[],
            rowsPerPage: 5,
            currentPage: 1,
            totalPages: 0,
            totalRows: 0,
            paginationCount: 14,

            logIdentification: '',
            logTypeId: '',
            logIp: '',
            description: '',

            firtsPage: 1,
            lastPage: 14
        }
    }

    filterInfo = async () => {

        await this.setState({
            currentPage: 1,
            firtsPage: 1,
            lastPage: 14,
        });

        this.getLogInfos();
    }

    getLogInfos = async () => {

        //https://localhost:44373/Log/GetLogInfo?Page=2&RowsPerpage=50&RowsPerpage=50
        var url = "https://localhost:44373/Log/GetLogInfo?";

        var parans = "";

        if (this.state.currentPage)
            parans += `Page=${this.state.currentPage}`;

        if (this.state.rowsPerPage)
            parans += `${(parans != "" ? "&" : "")}RowsPerPage=${this.state.rowsPerPage}`;

        if (this.state.logIdentification)
            parans += `${(parans != "" ? "&" : "")}LogIdentification=${this.state.logIdentification}`;

        if (this.state.logTypeId)
            parans += `${(parans != "" ? "&" : "")}LogTypeId=${this.state.logTypeId}`;

        if (this.state.logIp)
            parans += `${(parans != "" ? "&" : "")}LogIp=${this.state.logIp}`;

        if (this.state.description)
            parans += `${(parans != "" ? "&" : "")}Description=${this.state.description}`;

        url += parans;

        const obj = {
            method: "GET",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            }
        }

        await fetch(`${url}`, obj)
            .then((response) => response.json())
            .then((responseJson) => {
                //console.warn(responseJson);

                responseJson.data.map(x => {
                    if (this.state.logTypes) {

                        var logType = this.state.logTypes.find(y => y.id == x.logTypeId);

                        if (logType)
                            x.logTypeName = logType.name;
                    }
                })

                this.setState({
                    totalPages: responseJson.totalPages,
                    totalRows: responseJson.totalRows,
                    data: responseJson.data
                })
            })
            .catch((error) => {
                console.warn(error);
            })
    }

    getLogTypes = async () => {

        var url = "https://localhost:44373/Log/GetLogTypes";

        const obj = {
            method: "GET",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            }
        }

        var dropDownLogTypes = [];

        await fetch(`${url}`, obj)
            .then((response) => response.json())
            .then((responseJson) => {
                //console.warn(responseJson);

                dropDownLogTypes.push({ value: 0, label: 'Selecione logType' });

                responseJson.map(x => {
                    dropDownLogTypes.push({ value: x.id, label: x.name })
                });

                this.setState({
                    logTypes: responseJson,
                    dropDownLogTypes: dropDownLogTypes
                });
            })
            .catch((error) => {
                console.warn(error);
            })
    }

    async componentDidMount() {
        await this.getLogTypes()
        await this.getLogInfos()
    }

    _onSelect = async (item) => {
        await this.setState({
            logTypeId: item.value
        });

        await this.filterInfo();
    }

    handleKeyPress = (e) => {
        if (e.charCode == 13 || e.keyCode == 13) {
            this.filterInfo();
        }
    }

    render() {
        return (
            <div className="container"
                style={{
                    margin: 'auto',
                   'marginTop': '3%',
                    width: '100%',
                    'borderRadius': '10px',
                    padding: '10px',
                    height: '650px'
                }}>

                <div className="row" style={{'marginBottom':'10px'}}>
                    <div className="col-sm-12" style={{ textAlign: 'center' }}>
                        <h3><b>Registros de log</b></h3>
                    </div>
                </div>

                <FilterInfo
                    handleEnterPress={this.handleKeyPress}
                    setDescription={(value) => this.setState({ description: value })}
                    description={this.state.description}
                    setLogIdentification={(value) => this.setState({ logIdentification: value })}
                    logIdentification={this.state.logIdentification}
                    setLogIp={(value) => this.setState({ logIp: value })}
                    LogIp={this.state.logIp}
                    dropDownLogTypes={this.state.dropDownLogTypes}
                    onSelectDropDown={async (value) => { await this._onSelect(value); }}
                    filterInfo={this.filterInfo}
                />

                <TableInfo
                    data={this.state.data}

                    setFistLastPage={(value) => this.setState({ firtsPage: value.firtsPage, lastPage: value.lastPage }) }

                    setCurrentPage={async (value) => await this.setState({ currentPage: value }) }
                    getLogInfos={this.getLogInfos}

                    rowsPerPage={this.state.rowsPerPage}
                    currentPage={this.state.currentPage}
                    paginationCount={this.state.paginationCount}
                    firtsPage={this.state.firtsPage}
                    lastPage={this.state.lastPage}
                    totalPages={this.state.totalPages}
                    totalRows={this.state.totalRows}
                />
                
                

            </div>
        );
    }
}