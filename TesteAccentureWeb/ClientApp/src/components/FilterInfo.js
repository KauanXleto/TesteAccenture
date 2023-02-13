import React, { Component } from 'react';
import Dropdown from 'react-dropdown';
import { Container } from 'reactstrap';

export class FilterInfo extends Component {
    static displayName = FilterInfo.name;

  render () {
    return (
        <div style={{
            'padding': '10px',
            'boxShadow': 'rgb(219 219 219) 0px 0px 20px 0px',
            'borderRadius': '10px',
            'marginBottom': '10px'
        }}>

            <div className="row form-group">
                <div className="col-sm-6">
                    <input
                        type="text"
                        className="form-control"
                        placeholder="LogDescription"
                        id="LogDescription"
                        onKeyPress={this.props.handleEnterPress}
                        onChange={(e) => this.props.setDescription(e.target.value)}
                        value={this.props.description}>
                    </input>
                </div>
                <div className="col-sm-6">
                    <input
                        type="text"
                        className="form-control"
                        placeholder="LogIdentification"
                        id="LogIdentification"
                        onKeyPress={this.props.handleEnterPress}
                        onChange={(e) => this.props.setLogIdentification(e.target.value)}
                        value={this.props.logIdentification}>
                    </input>
                </div>
            </div>

            <div className="row form-group">

                <div className="col-sm-6">
                    <input
                        type="text"
                        className="form-control"
                        placeholder="LogIp"
                        id="LogIp"
                        onKeyPress={this.props.handleEnterPress}
                        onChange={(e) => this.props.setLogIp(e.target.value)}
                        value={this.props.logIp}>
                    </input>
                </div>
                <div className="col-sm-4">
                    <Dropdown
                        options={this.props.dropDownLogTypes}
                        onChange={(e) => this.props.onSelectDropDown(e)}
                        value={this.props.dropDownLogTypes[0]}
                        placeholder="Select an option" />
                </div>
                <div className="col-sm-2">
                    <button
                        onClick={this.props.filterInfo}
                        type="button"
                        className="btn btn-primary float-right">
                        Consultar
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-search" viewBox="0 0 16 16" style={{ 'verticalAlign': 'middle', 'marginLeft': '5px' }}>
                            <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z" />
                        </svg>
                    </button>
                </div>


            </div>
        </div>
    );
  }
}
