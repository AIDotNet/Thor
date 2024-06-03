import { Avatar, type AvatarProps } from '@lobehub/ui';
import { createStyles } from 'antd-style';
import { memo, } from 'react';

const useStyles = createStyles(({ css, token }) => ({
  clickable: css`
    position: relative;
    transition: all 200ms ease-out 0s;

    &::before {
      content: '';

      position: absolute;
      transform: skewX(-45deg) translateX(-400%);

      overflow: hidden;

      box-sizing: border-box;
      width: 25%;
      height: 100%;

      background: rgba(255, 255, 255, 50%);

      transition: all 200ms ease-out 0s;
    }

    &:hover {
      box-shadow: 0 0 0 2px ${token.colorPrimary};

      &::before {
        transform: skewX(-45deg) translateX(400%);
      }
    }
  `,
}));

export interface UserAvatarProps extends AvatarProps {
  clickable?: boolean;
  user?: any;
}

const UserAvatar = memo<UserAvatarProps>(
  ({ size = 40, avatar, background, clickable, user, className, style, ...rest }) => {
    const { styles, cx } = useStyles();

    function getAvatar() {
      return user?.avatar || '/vite.svg'
    }

    return (
      <Avatar
        avatar={getAvatar()}
        background={background}
        className={cx(clickable && styles.clickable, className)}
        size={size}
        style={{ flex: 'none', ...style }}
        unoptimized
        {...rest}
      />
    );
  },
);

UserAvatar.displayName = 'UserAvatar';

export default UserAvatar;
