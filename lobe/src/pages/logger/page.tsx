import { useEffect, useState } from "react";
import styled from "styled-components";
import {
  message,
  Dropdown,
  Button,
  Input,
  Select,
  DatePicker,
  Table,
  Card,
} from "antd";
import { getLoggers, viewConsumption } from "../../services/LoggerService";
import { Tag } from "@lobehub/ui";
import { renderQuota } from "../../utils/render";

const Header = styled.header``;

export default function LoggerPage() {
  const [data, setData] = useState<any[]>([]);
  const [total, setTotal] = useState<number>(0);
  const [loading, setLoading] = useState<boolean>(false);
  const [consume, setConsume] = useState<number>(0);
  const [consumeLoading, setConsumeLoading] = useState<boolean>(false);
  const [input, setInput] = useState({
    page: 1,
    pageSize: 10,
    type: -1 | 1 | 2 | 3,
    model: "",
    startTime: "",
    endTime: "",
    keyword: "",
  });

  function timeString(totalTime: number) {
    // ms转换为s，并且可读，
    let s = totalTime / 1000;
    let m = Math.floor(s / 60);
    s = Math.floor(s % 60);
    return `${m}分${s}秒`;
  }

  const columns = [
    {
      title: "时间",
      dataIndex: "createdAt",
      key: "createdAt",
      width: "180px",
    },
    {
      disable: !(localStorage.getItem("role") === "admin"),
      title: "渠道",
      dataIndex: "channelName",
      width: "190px",
      key: "channelName",
      render: (value: any) => {
        return value && <Tag color="blue">{value}</Tag>;
      },
    },
    {
      title: "用户",
      dataIndex: "userName",
      width: "90px",
      key: "userName",
      render: (value: any) => {
        return value && <Tag color="blue">{value}</Tag>;
      },
    },
    {
      title: "令牌名称",
      dataIndex: "tokenName",
      width: "190px",
      key: "tokenName",
    },
    {
      title: "模型",
      dataIndex: "modelName",
      width: "180px",
      key: "modelName",
      render: (value: any) => {
        return (
          value && (
            <Tag
              onClick={() => {
                navigator.clipboard
                  .writeText(value)
                  .then(() => {
                    message.success({
                      content: "复制成功",
                    });
                  })
                  .catch(() => {
                    message.error({
                      content: "复制失败",
                    });
                  });
              }}
            >
              {value}
            </Tag>
          )
        );
      },
    },
    {
      title: "用时",
      dataIndex: "duration",
      key: "duration",
      width: "120px",
      render: (_: any, item: any) => {
        return (
          <>
            <Tag color="pink">{timeString(item.totalTime)}ms</Tag>
            <Tag color="gold">{item.stream ? "流式" : "非流式"}</Tag>
          </>
        );
      },
    },
    {
      title: "提示tokens",
      dataIndex: "promptTokens",
      key: "promptTokens",
      width: "120px",
    },
    {
      title: "完成tokens",
      dataIndex: "completionTokens",
      key: "completionTokens",
      width: "120px",
    },
    {
      title: "请求IP地址",
      dataIndex: "ip",
      key: "ip",
      width: "120px",
    },
    {
      title: "额度",
      dataIndex: "quota",
      key: "quota",
      width: "120px",
      render: (value: any) => {
        return value && <Tag color="green">{renderQuota(value, 6)}</Tag>;
      },
    },
    {
      title: "详情",
      dataIndex: "content",
      key: "content",
      width: "250px",
    },
    {
      title: "客户端信息",
      dataIndex: "userAgent",
      width: "380px",
      key: "userAgent",
    },
  ];

  function loadData() {
    setLoading(true);

    getLoggers(input)
      .then((res) => {
        if (res.success) {
          setData(res.data.items);
          setTotal(res.data.total);
        } else {
          message.error({
            content: res.message,
          });
        }
      })
      .finally(() => {
        setLoading(false);
      });
  }

  function loadViewConsumption() {
    setConsumeLoading(true);
    viewConsumption(input)
      .then((res) => {
        if (res.success) {
          setConsume(res.data);
        } else {
          message.error({
            content: res.message,
          });
        }
      })
      .finally(() => {
        setConsumeLoading(false);
      });
  }

  useEffect(() => {
    loadData();
  }, [input.page, input.pageSize]);

  return (
    <div
      style={{
        margin: "10px",
        height: "80vh",
        overflow: "auto",
        width: "100%",
      }}
    >
      <Header>
        <Card
          style={{
            float: "left",
          }}
          bodyStyle={{
            padding: "8px",
          }}
        >
          <span>
            区间消费:{" "}
            <span
              style={{
                fontWeight: "bold",
              }}
            >
              {renderQuota(consume, 4)}
            </span>
          </span>

          <Button
            type="text"
            onClick={() => {
              loadViewConsumption();
            }}
            loading={consumeLoading}
            style={{
              marginLeft: "0.5rem",
            }}
          >
            查看消费
          </Button>
        </Card>
        <Dropdown
          menu={{
            items: [
              {
                key: 1,
                label: "删除选中令牌",
                onClick: () => {
                  console.log("删除选中令牌");
                },
              },
            ],
          }}
        >
          <Button
            style={{
              float: "right",
            }}
          >
            操作
          </Button>
        </Dropdown>
        <Button
          onClick={() => loadData()}
          style={{
            marginRight: "0.5rem",
            float: "right",
          }}
        >
          搜索
        </Button>
        <Input
          value={input.model}
          onChange={(e) => {
            setInput({
              ...input,
              model: e.target.value,
            });
          }}
          style={{
            marginRight: "0.5rem",
            float: "right",
            width: "5rem",
          }}
          placeholder="模型名称"
        />
        <Select
          style={{
            marginRight: "0.5rem",
            float: "right",
            width: "5rem",
          }}
          value={input.type}
          onChange={(e: any) => {
            setInput({
              ...input,
              type: e,
            });
          }}
        >
          <Select.Option value={-1}>全部</Select.Option>
          <Select.Option value={1}>消费</Select.Option>
          <Select.Option value={2}>充值</Select.Option>
          <Select.Option value={3}>系统</Select.Option>
        </Select>
        <Input
          value={input.keyword}
          onChange={(e) => {
            setInput({
              ...input,
              keyword: e.target.value,
            });
          }}
          style={{
            marginRight: "0.5rem",
            float: "right",
            width: "5rem",
          }}
          placeholder="关键字"
        />

        <DatePicker
          value={input.startTime}
          onChange={(e: any) => {
            setInput({
              ...input,
              startTime: e,
            });
          }}
          style={{
            marginRight: "0.5rem",
            width: "10rem",
            float: "right",
          }}
          placeholder="开始时间"
        />
        <DatePicker
          value={input.endTime}
          onChange={(e: any) => {
            setInput({
              ...input,
              endTime: e,
            });
          }}
          style={{
            marginRight: "0.5rem",
            width: "10rem",
            float: "right",
          }}
          placeholder="结束时间"
        />
      </Header>
      <Table
        scroll={{
          y: 800,
          x: 1800,
        }}
        loading={loading}
        style={{
          marginTop: "1rem",
        }}
        columns={columns.filter(
          (item) => item.disable == false || item.disable == undefined
        )}
        dataSource={data}
        pagination={{
          total: total,
          pageSize: input.pageSize,
          defaultPageSize: input.page,
          onChange: (page, pageSize) => {
            // 修改input以后获取数据
            setInput({
              ...input,
              page,
              pageSize,
            });
          },
        }}
      />
    </div>
  );
}
