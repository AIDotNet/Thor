import { useEffect, useState } from "react";
import styled from "styled-components";
import {
  message,
  Button,
  Input,
  Select,
  DatePicker,
  Table,
  Card,
  Skeleton
} from "antd";
import { getLoggers, viewConsumption } from "../../services/LoggerService";
import { Tag, Tooltip } from "@lobehub/ui";
import { renderQuota } from "../../utils/render";
import dayjs from "dayjs";
import { motion, AnimatePresence } from "framer-motion";

const Header = styled.header`
  margin-bottom: 16px;
  transition: all 0.3s ease;
`;

const AnimatedCard = styled(Card)`
  transition: all 0.3s ease;
  &:hover {
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    transform: translateY(-2px);
  }
`;

const AnimatedButton = styled(Button)`
  transition: all 0.2s ease;
  &:hover {
    transform: scale(1.05);
  }
  &:active {
    transform: scale(0.95);
  }
`;

const AnimatedTag = styled(Tag)`
  transition: all 0.2s ease;
  &:hover {
    transform: scale(1.1);
  }
`;

const PageContainer = styled(motion.div)`
  margin: 10px;
  width: 100%;
  height: 100%;
`;

const SearchBar = styled(motion.div)`
  display: flex;
  flex-direction: row-reverse;
  flex-wrap: wrap;
  gap: 8px;
  margin-bottom: 16px;
`;

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
    startTime: null,
    endTime: null,
    keyword: "",
  } as {
    page: number;
    pageSize: number;
    type: -1 | 1 | 2 | 3;
    model: string;
    startTime: string | null;
    endTime: string | null;
    keyword: string;
    organizationId?: string;
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
      fixed: "left",
      dataIndex: "createdAt",
      key: "createdAt",
      width: "180px",
    },
    {
      title: "消费",
      dataIndex: "quota",
      fixed: "left",
      key: "quota",
      width: "120px",
      render: (value: any) => {
        return value && (
          <AnimatedTag color="green">{renderQuota(value, 6)}</AnimatedTag>
        );
      },
    },
    {
      disable: !(localStorage.getItem("role") === "admin"),
      title: "渠道",
      dataIndex: "channelName",
      width: "190px",
      key: "channelName",
      render: (value: any) => {
        return value && <AnimatedTag color="blue">{value}</AnimatedTag>;
      },
    },
    {
      title: "用户",
      dataIndex: "userName",
      width: "90px",
      key: "userName",
      render: (value: any) => {
        return value && <AnimatedTag color="blue">{value}</AnimatedTag>;
      },
    },
    {
      title: "令牌名称",
      dataIndex: "tokenName",
      width: "190px",
      key: "tokenName",
    },
    {
      // organizationId
      title: '组织id',
      dataIndex: 'organizationId',
      key: 'organizationId'
    },
    {
      title: "模型",
      dataIndex: "modelName",
      width: "180px",
      key: "modelName",
      render: (value: any) => {
        return (
          value && (
            <AnimatedTag
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
            </AnimatedTag>
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
            <AnimatedTag color="pink">{timeString(item.totalTime)}ms</AnimatedTag>
            <AnimatedTag color="gold">{item.stream ? "流式" : "非流式"}</AnimatedTag>
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
      title: "详情",
      dataIndex: "content",
      key: "content",
      width: "250px",
    },
    {
      title: "客户端信息",
      dataIndex: "userAgent",
      key: "userAgent",
      render: (value: any) => {
        // 如果value超过10个字符，就截取前10个字符
        if (value && value.length > 10) {
          return <Tooltip title={value}>{value.slice(0, 10)}...</Tooltip>;
        }
        return <Tooltip title={value}>{value}</Tooltip>;
      }
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

  // 数字动画组件
  const AnimatedNumber = ({ value }: { value: number }) => {
    return (
      <motion.span
        key={value}
        initial={{ opacity: 0, y: 10 }}
        animate={{ opacity: 1, y: 0 }}
        exit={{ opacity: 0, y: -10 }}
        transition={{ duration: 0.3 }}
        style={{ fontWeight: "bold", display: "inline-block" }}
      >
        {renderQuota(value, 4)}
      </motion.span>
    );
  };

  return (
    <PageContainer
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      transition={{ duration: 0.5 }}
    >
      <Header>
        <AnimatedCard
          style={{
            float: "left",
          }}
          bodyStyle={{
            padding: "8px",
          }}
        >
          <span>
            区间消费:{" "}
            <AnimatePresence mode="wait">
              {consumeLoading ? (
                <Skeleton.Button active size="small" style={{ width: "80px" }} />
              ) : (
                <AnimatedNumber value={consume} />
              )}
            </AnimatePresence>
          </span>

          <AnimatedButton
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
          </AnimatedButton>
        </AnimatedCard>
        
        <SearchBar
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ 
            duration: 0.5, 
            staggerChildren: 0.1, 
            delayChildren: 0.2 
          }}
        >
          <motion.div whileHover={{ scale: 1.02 }} whileTap={{ scale: 0.98 }}>
            <AnimatedButton
              onClick={() => loadData()}
              style={{ marginRight: "0.5rem" }}
              type="primary"
            >
              搜索
            </AnimatedButton>
          </motion.div>
          
          <motion.div whileHover={{ scale: 1.02 }}>
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
                width: "7rem",
              }}
              placeholder="模型名称"
              allowClear
            />
          </motion.div>
          
          <motion.div whileHover={{ scale: 1.02 }}>
            <Select
              style={{
                marginRight: "0.5rem",
                width: "6rem",
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
              <Select.Option value={4}>新增用户</Select.Option>
            </Select>
          </motion.div>
          
          <motion.div whileHover={{ scale: 1.02 }}>
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
                width: "7rem",
              }}
              placeholder="关键字"
              allowClear
            />
          </motion.div>
          
          <motion.div whileHover={{ scale: 1.02 }}>
            <Input
              value={input.organizationId}
              onChange={(e) => {
                setInput({
                  ...input,
                  organizationId: e.target.value,
                });
              }}
              style={{
                marginRight: "0.5rem",
                width: "7rem",
              }}
              placeholder="组织Id"
              allowClear
            />
          </motion.div>
          
          <motion.div whileHover={{ scale: 1.02 }}>
            <DatePicker
              value={input.endTime ? dayjs(input.endTime) : null}
              onChange={(e: any) => {
                setInput({
                  ...input,
                  endTime: e ? e.format("YYYY-MM-DD HH:mm:ss") : null,
                });
              }}
              style={{
                marginRight: "0.5rem",
                width: "10rem",
              }}
              placeholder="结束时间"
              allowClear
            />
          </motion.div>
          
          <motion.div whileHover={{ scale: 1.02 }}>
            <DatePicker
              value={input.startTime ? dayjs(input.startTime) : null}
              onChange={(e: any) => {
                setInput({
                  ...input,
                  startTime: e ? e.format("YYYY-MM-DD HH:mm:ss") : null,
                });
              }}
              style={{
                marginRight: "0.5rem",
                width: "10rem",
              }}
              placeholder="开始时间"
              allowClear
            />
          </motion.div>
        </SearchBar>
      </Header>
      
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5, delay: 0.3 }}
      >
        <Table
          scroll={{
            x: "max-content",
            y: "calc(100vh - 350px)",
          }}
          loading={loading}
          style={{
            marginTop: "1rem",
          }}
          columns={columns.filter(
            (item) => item.disable == false || item.disable == undefined
          ) as any}
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
          rowClassName={(_, index) => (index % 2 === 0 ? "table-row-light" : "table-row-dark")}
          onRow={() => {
            return {
              style: {
                transition: "background-color 0.3s ease",
              },
              onMouseEnter: (e) => {
                e.currentTarget.style.backgroundColor = "rgba(0, 112, 204, 0.08)";
              },
              onMouseLeave: (e) => {
                e.currentTarget.style.backgroundColor = "";
              },
            };
          }}
        />
      </motion.div>
    </PageContainer>
  );
}